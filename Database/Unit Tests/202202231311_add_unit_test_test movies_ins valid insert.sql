IF OBJECT_ID('[unit_tests].[test movies_ins valid insert]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test movies_ins valid insert];
GO

-- test that the insert occurs correctly
CREATE PROCEDURE [unit_tests].[test movies_ins valid insert]
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

    DECLARE @title [VARCHAR](200) = 'The Matrix';
    DECLARE @description [VARCHAR](2000) = 'Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.';
    DECLARE @movie_type_id [INT] = 1;
    DECLARE @duration [INT] = 136;
    DECLARE @release_date [DATETIME] = DATEFROMPARTS(1999, 03, 30);
    DECLARE @added_date [DATETIME] = GETDATE();
    DECLARE @owned [BIT] = 1;

    EXEC tSQLt.FakeTable @TableName = 'movies', @Identity = 1;
    exec tSQLt.ApplyConstraint @TableName = 'movies', @ConstraintName = 'FK_movies_movie_type_id', @NoCascade = 1;

    --Act
    DECLARE @id INT;
    EXEC @id = dbo.movies_ins @title = @title,
                              @description = @description,
                              @movie_type_id = @movie_type_id,
                              @duration = @duration,
                              @release_date = @release_date,
                              @added_date = @added_date,
                              @owned = @owned;

    PRINT @id;
    SELECT id,
           title,
           [description],
           movie_type_id,
           duration,
           release_date,
           added_date,
           owned
    INTO dbo.actual
    FROM dbo.movies
    WHERE title = @title;

    --Assert
    CREATE TABLE dbo.expected
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

    INSERT INTO dbo.expected
    (
        title,
        [description],
        movie_type_id,
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT @title,         -- title - varchar(200)
           @description,   -- description - varchar(2000)
           @movie_type_id, -- movie_type_id - int
           @duration,      -- duration - int
           @release_date,  -- release_date - datetime
           @added_date,    -- added_date - datetime
           @owned;         -- owned - bit

    EXEC tSQLt.AssertEqualsTable @Expected = 'expected', @Actual = 'actual', @Message = 'Inserted record does not match expected record.';
END;





