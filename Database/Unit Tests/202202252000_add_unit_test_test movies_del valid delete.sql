IF OBJECT_ID('[unit_tests].[test movies_del valid delete]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test movies_del valid delete];
GO

-- test that movies_del deletes the movie correctly
CREATE PROCEDURE [unit_tests].[test movies_del valid delete]
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
    
    DECLARE @id INT = 2;
    DECLARE @title [VARCHAR](200) = '47 Ronin';
    DECLARE @description [VARCHAR](2000) = 'Kai—an outcast—joins Oishi, the leader of 47 outcast samurai. Together they seek vengeance upon the treacherous overlord who killed their master and banished their kind. To restore honour to their homeland, the warriors embark upon a quest that challenges them with a series of trials that would destroy ordinary warriors.';
    DECLARE @movie_type_id [INT] = 1;
    DECLARE @wrong_duration [INT] = 20;
    DECLARE @correct_duration [INT] = 119;
    DECLARE @release_date [DATETIME] = DATEFROMPARTS(2013, 12, 06);
    DECLARE @added_date [DATETIME] = GETDATE();
    DECLARE @owned [BIT] = 1;

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
    SELECT @id,             -- id - int
           @title,          -- title - varchar(200)
           @description,    -- description - varchar(2000)
           @movie_type_id,  -- movie_type_id - int
           @wrong_duration, -- duration - int
           @release_date,   -- release_date - datetime
           @added_date,     -- added_date - datetime
           @owned           -- owned - bit
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
    EXEC dbo.movies_del @id = @id

    --Assert
    DECLARE @deleted BIT = CASE WHEN EXISTS((SELECT TOP(1) duration FROM dbo.movies WHERE id = @id ORDER BY id)) THEN 1 ELSE 0 END;

    EXEC tSQLt.assertEquals @Expected = 0, @Actual = @deleted, @Message = 'Record not deleted.';
END;





