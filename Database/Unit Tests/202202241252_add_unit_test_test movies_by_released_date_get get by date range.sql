IF OBJECT_ID('[unit_tests].[test movies_by_released_date_get get get by date range]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test movies_by_released_date_get get get by date range];
GO

-- test that movies_by_released_date_get gets all the owned movies
CREATE PROCEDURE [unit_tests].[test movies_by_released_date_get get get by date range]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

    EXEC tSQLt.FakeTable @TableName = 'movie_type';

    INSERT INTO dbo.movie_type
    (
        [id],
        [type]
    )
    SELECT 1, 'dvd';

    INSERT INTO dbo.movie_type
    (
        [id],
        [type]
    )
    SELECT 2, 'blu ray';

    DECLARE @from_date DATETIME = DATEFROMPARTS(1998, 01, 01);
    DECLARE @to_date DATETIME = DATEFROMPARTS(2013, 12, 07);

    EXEC tSQLt.FakeTable @TableName = 'movies';
    EXEC tSQLt.ApplyConstraint @TableName = 'movies', @ConstraintName = 'FK_movies_movie_type_id', @NoCascade = 1;

    INSERT INTO dbo.movies
    (
        id,
        title,
        [description],
        movie_type_id,
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT 1,                                                                                                                                                                                       -- id - int
           'The Matrix',                                                                                                                                                                            -- title - varchar(200)
           'Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.', -- description - varchar(2000)
           2,                                                                                                                                                                                       -- movie_type_id - int
           136,                                                                                                                                                                                     -- duration - int
           DATEFROMPARTS(1999, 03, 30),                                                                                                                                                             -- release_date - datetime
           GETDATE(),                                                                                                                                                                               -- added_date - datetime
           1                                                                                                                                                                                        -- owned - bit
    ;

    INSERT INTO dbo.movies
    (
        id,
        title,
        [description],
        movie_type_id,
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT 2,                                                                                                                                                                                                                                                                                                                                    -- id - int
           '47 Ronin',                                                                                                                                                                                                                                                                                                                           -- title - varchar(200)
           'Kai—an outcast—joins Oishi, the leader of 47 outcast samurai. Together they seek vengeance upon the treacherous overlord who killed their master and banished their kind. To restore honour to their homeland, the warriors embark upon a quest that challenges them with a series of trials that would destroy ordinary warriors.', -- description - varchar(2000)
           1,                                                                                                                                                                                                                                                                                                                                    -- movie_type_id - int
           119,                                                                                                                                                                                                                                                                                                                                  -- duration - int
           DATEFROMPARTS(2013, 12, 06),                                                                                                                                                                                                                                                                                                          -- release_date - datetime
           GETDATE(),                                                                                                                                                                                                                                                                                                                            -- added_date - datetime
           1                                                                                                                                                                                                                                                                                                                                     -- owned - bit
    ;

    INSERT INTO dbo.movies
    (
        id,
        title,
        [description],
        movie_type_id,
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT 3,                                                                                                        -- id - int
           'John Wick',                                                                                              -- title - varchar(200)
           'Ex-hitman John Wick comes out of retirement to track down the gangsters that took everything from him.', -- description - varchar(2000)
           1,                                                                                                        -- movie_type_id - int
           101,                                                                                                      -- duration - int
           DATEFROMPARTS(2014, 10, 22),                                                                              -- release_date - datetime
           GETDATE(),                                                                                                -- added_date - datetime
           1                                                                                                         -- owned - bit
    ;

    --Act
    CREATE TABLE dbo.actual
    (
        [id] [INT] IDENTITY(1, 1) NOT NULL,
        [title] [VARCHAR](200) NOT NULL,
        [description] [VARCHAR](2000) NOT NULL,
        [movie_type_id] [INT] NOT NULL,
        [duration] [INT] NOT NULL,
        [release_date] [DATETIME] NOT NULL,
        [added_date] [DATETIME] NOT NULL,
        [owned] [BIT] NOT NULL,
    );

    INSERT INTO dbo.actual
    (
        id,
        title,
        [description],
        movie_type_id,
        duration,
        release_date,
        added_date,
        owned
    )
    EXEC dbo.movies_by_released_date_get @from_date = @from_date, @to_date = @to_date;

    --Assert
    SELECT id,
           title,
           [description],
           movie_type_id,
           duration,
           release_date,
           added_date,
           owned
    INTO dbo.expected
    FROM dbo.movies
    WHERE id IN (1, 2);

    EXEC tSQLt.AssertEqualsTable @Expected = 'expected', @Actual = 'actual', @Message = 'Get record does not match expected record.';
END;





