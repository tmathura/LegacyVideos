IF OBJECT_ID('[unit_tests].[test series_del valid delete]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_del valid delete];
GO

-- test that series_del deletes the series correctly
CREATE PROCEDURE [unit_tests].[test series_del valid delete]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

    DECLARE @id INT = 2;
    DECLARE @wrong_title [VARCHAR](200) = 'Supernatural: The Animations';
    DECLARE @correct_title [VARCHAR](200) = 'Supernatural: The Animation';
    DECLARE @description [VARCHAR](2000) = 'Supernatural the Animation will not only remake the best episodes from the live-action version, but also depict original episodes. These original episodes will include prologues of the Winchester brothers’ childhood, anime-only enemies, and episodes featuring secondary characters from the original series.';
    DECLARE @release_date [DATETIME] = DATEFROMPARTS(2011, 01, 12);
    DECLARE @added_date [DATETIME] = GETDATE();
    DECLARE @ended [BIT] = 1;
    DECLARE @owned [BIT] = 1;
    DECLARE @anime [BIT] = 1;

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
    SELECT @id,           -- id - int
           @wrong_title,  -- title - varchar(200)
           @description,  -- description - varchar(2000)
           @release_date, -- release_date - datetime
           @added_date,   -- added_date - datetime
           @ended,        -- ended - bit
           @owned,        -- owned - bit
           @anime         -- anime - bit                                                                                                                                                                                                                                                                                                                            -- owned - bit
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
    SELECT 3,                                                                                                                                                                                                                                                                                                                 -- id - int
           'Supergirl',                                                                                                                                                                                                                                                                                                       -- title - varchar(200)
           'As Superman’s cousin, Kara Danvers, also known as Supergirl, balances her work as a reporter for CatCo Worldwide Media with her work for the Department of Extra-Normal Operations, a super-secret government organization whose mission is to keep National City – and the Earth – safe from sinister threats.', -- description - varchar(2000)
           DATEFROMPARTS(2015, 10, 26),                                                                                                                                                                                                                                                                                       -- release_date - datetime
           GETDATE(),                                                                                                                                                                                                                                                                                                         -- added_date - datetime
           1,                                                                                                                                                                                                                                                                                                                 -- ended - bit
           1,                                                                                                                                                                                                                                                                                                                 -- owned - bit
           0                                                                                                                                                                                                                                                                                                                  -- anime - bit
    ;

    --Act
    EXEC dbo.series_del @id = @id;

    --Assert
    DECLARE @deleted BIT = CASE WHEN EXISTS((SELECT TOP (1) title FROM dbo.series WHERE id = @id ORDER BY id)) THEN 1 ELSE 0 END;
    
    EXEC tSQLt.assertEquals @Expected = 0, @Actual = @deleted, @Message = 'Record not deleted.';
END;





