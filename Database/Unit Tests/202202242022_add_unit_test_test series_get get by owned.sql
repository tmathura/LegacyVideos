IF OBJECT_ID('[unit_tests].[test series_get get by owned]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_get get by owned];
GO

-- test that series_get gets all the owned series
CREATE PROCEDURE [unit_tests].[test series_get get by owned]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

    DECLARE @owned INT = 1;

    EXEC tSQLt.FakeTable @TableName = 'series';

    INSERT INTO dbo.series
    (
        id,
        title,
        [description],
        release_date,
        added_date,
        ended,
        owned,
        anime
    )
    SELECT 1,                                                                                                                                                                     -- id - int
           'Supernatural',                                                                                                                                                        -- title - varchar(200)
           'Two brothers follow their father''s footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.', -- description - varchar(2000)
           DATEFROMPARTS(2005, 09, 13),                                                                                                                                           -- release_date - datetime
           GETDATE(),                                                                                                                                                             -- added_date - datetime
           1,                                                                                                                                                                     -- ended - bit
           1,                                                                                                                                                                     -- owned - bit
           0                                                                                                                                                                      -- anime - bit
    ;

    INSERT INTO dbo.series
    (
        id,
        title,
        [description],
        release_date,
        added_date,
        ended,
        owned,
        anime
    )
    SELECT 2,                                                                                                                                                                                                                                                                                                                    -- id - int
           'Supernatural: The Animation',                                                                                                                                                                                                                                                                                        -- title - varchar(200)
           'Supernatural the Animation will not only remake the best episodes from the live-action version, but also depict original episodes. These original episodes will include prologues of the Winchester brothers’ childhood, anime-only enemies, and episodes featuring secondary characters from the original series.', -- description - varchar(2000)
           DATEFROMPARTS(2011, 01, 12),                                                                                                                                                                                                                                                                                          -- release_date - datetime
           GETDATE(),
           1,                                                                                                                                                                                                                                                                                                                    -- ended - bit
           1,                                                                                                                                                                                                                                                                                                                    -- owned - bit
           1                                                                                                                                                                                                                                                                                                                     -- anime - bit                                                                                                                                                                                                                                                                                                                            -- owned - bit
    ;

    --Act
    CREATE TABLE dbo.actual
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

    INSERT INTO dbo.actual
    (
        id,
        title,
        [description],
        release_date,
        added_date,
        ended,
        owned,
        anime
    )
    EXEC dbo.series_get @id = NULL, @title = NULL, @owned = @owned;

    --Assert
    SELECT id,
           title,
           [description],
           release_date,
           added_date,
           ended,
           owned,
           anime
    INTO dbo.expected
    FROM dbo.series
    WHERE owned = @owned;

    EXEC tSQLt.AssertEqualsTable @Expected = 'expected', @Actual = 'actual', @Message = 'Get record does not match expected record.';
END;





