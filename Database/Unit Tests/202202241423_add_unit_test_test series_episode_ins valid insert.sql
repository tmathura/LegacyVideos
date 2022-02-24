IF OBJECT_ID('[unit_tests].[test series_episode_ins valid insert]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_episode_ins valid insert];
GO

-- test that the insert occurs correctly
CREATE PROCEDURE [unit_tests].[test series_episode_ins valid insert]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

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
           0;                                                                                                                                                                     -- anime - bit

    DECLARE @series_id INT = 1;
    DECLARE @episode_number INT = 1;
    DECLARE @title [VARCHAR](200) = 'Pilot';
    DECLARE @description [VARCHAR](2000) = 'Sam is about to graduate from college and has an interview set up to join one of the most prestigious law schools in the country. His brother Dean, whom he has not seen since he went to college, shows up in the middle of the night and tells him their father is missing while on a hunting trip. Leaving his girlfriend behind to find their dad, Sam joins Dean in an effort to find their father in a little town called Jericho, where unmarried men disappear without a trace.';
    DECLARE @duration INT = 47;
    DECLARE @release_date [DATETIME] = DATEFROMPARTS(2005, 09, 14);
    DECLARE @added_date [DATETIME] = GETDATE();
    DECLARE @owned [BIT] = 1;

    EXEC tSQLt.FakeTable @TableName = 'series_episode', @Identity = 1;
    EXEC tSQLt.ApplyConstraint @TableName = 'series_episode', @ConstraintName = 'FK_series_episode_series_id', @NoCascade = 1;

    --Act
    DECLARE @id INT;
    EXEC @id = dbo.series_episode_ins @series_id = @series_id,
                                      @episode_number = @episode_number,
                                      @title = @title,
                                      @description = @description,
                                      @duration = @duration,
                                      @release_date = @release_date,
                                      @added_date = @added_date,
                                      @owned = @owned;

    SELECT id,
           series_id,
           episode_number,
           title,
           [description],
           duration,
           release_date,
           added_date,
           owned
    INTO dbo.actual
    FROM dbo.series_episode
    WHERE [description] = @description;

    --Assert
    CREATE TABLE dbo.expected
    (
        [id] [INT] IDENTITY(1, 1) NOT NULL,
        [series_id] [INT] NOT NULL,
        [title] [VARCHAR](200) NOT NULL,
        [episode_number] [INT] NOT NULL,
        [description] [VARCHAR](2000) NOT NULL,
        [duration] [INT] NOT NULL,
        [release_date] [DATETIME] NOT NULL,
        [added_date] [DATETIME] NOT NULL,
        [owned] [BIT] NOT NULL,
    );

    INSERT INTO dbo.expected
    (
        series_id,
        episode_number,
        title,
        [description],
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT @series_id,    -- title - varchar(200)
           @episode_number, -- episode_number - int
           @title,          -- title - varchar(200)
           @description,  -- description - varchar(2000)
           @duration,     -- duration - int
           @release_date, -- release_date - datetime
           @added_date,   -- added_date - datetime
           @owned;        -- owned - bit

    EXEC tSQLt.AssertEqualsTable @Expected = 'expected', @Actual = 'actual', @Message = 'Inserted record does not match expected record.';
END;





