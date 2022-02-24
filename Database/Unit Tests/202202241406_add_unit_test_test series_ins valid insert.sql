IF OBJECT_ID('[unit_tests].[test series_ins valid insert]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_ins valid insert];
GO

-- test that the insert occurs correctly
CREATE PROCEDURE [unit_tests].[test series_ins valid insert]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

    DECLARE @title [VARCHAR](200) = 'Supernatural';
    DECLARE @description [VARCHAR](2000) = 'Two brothers follow their father''s footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.';
    DECLARE @release_date [DATETIME] = DATEFROMPARTS(2005, 09, 13);
    DECLARE @added_date [DATETIME] = GETDATE();
    DECLARE @ended [BIT] = 1;
    DECLARE @owned [BIT] = 1;
    DECLARE @anime [BIT] = 0;

    EXEC tSQLt.FakeTable @TableName = 'series', @Identity = 1;

    --Act
    DECLARE @id INT;
    EXEC @id = dbo.series_ins @title = @title,
                              @description = @description,
                              @release_date = @release_date,
                              @added_date = @added_date,
                              @ended = @ended,
                              @owned = @owned,
                              @anime = @anime;

    SELECT id,
           title,
           [description],
           release_date,
           added_date,
           ended,
           owned,
           anime
    INTO dbo.actual
    FROM dbo.series
    WHERE title = @title;

    --Assert
    CREATE TABLE dbo.expected
    (
        [id] [INT] IDENTITY(1, 1) NOT NULL,
        [title] [VARCHAR](200) NOT NULL,
        [description] [VARCHAR](2000) NOT NULL,
        [release_date] [DATETIME] NOT NULL,
        [added_date] [DATETIME] NOT NULL,
        [ended] [BIT] NOT NULL,
        [owned] [BIT] NOT NULL,
        [anime] [BIT] NOT NULL,
    );

    INSERT INTO dbo.expected
    (
        title,
        [description],
        release_date,
        added_date,
        ended,
        owned,
        anime
    )
    SELECT @title,         -- title - varchar(200)
           @description,   -- description - varchar(2000)
           @release_date,  -- release_date - datetime
           @added_date,    -- added_date - datetime
           @ended,         -- ended - bit
           @owned,         -- owned - bit
           @anime;         -- anime - bit

    EXEC tSQLt.AssertEqualsTable @Expected = 'expected', @Actual = 'actual', @Message = 'Inserted record does not match expected record.';
END;





