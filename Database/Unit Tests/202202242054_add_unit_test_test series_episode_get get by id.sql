IF OBJECT_ID('[unit_tests].[test series_episode_get get by id]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_episode_get get by id];
GO

-- test that series_episode_get gets the series episode by id
CREATE PROCEDURE [unit_tests].[test series_episode_get get by id]
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
           0                                                                                                                                                                      -- anime - bit
    ;

    DECLARE @id INT = 1;

    EXEC tSQLt.FakeTable @TableName = 'series_episode';
    EXEC tSQLt.ApplyConstraint @TableName = 'series_episode', @ConstraintName = 'FK_series_episode_series_id', @NoCascade = 1;
    EXEC tSQLt.ApplyConstraint @TableName = 'series_episode', @ConstraintName = 'IX_series_id_episode_number';

    INSERT INTO dbo.series_episode
    (
        id,
        series_id,
        episode_number,
        title,
        [description],
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT @id,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       -- id - int
           1,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         -- title - varchar(200)
           1,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         -- episode_number - int
           'Pilot',                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   -- title - varchar(200)
           'Sam is about to graduate from college and has an interview set up to join one of the most prestigious law schools in the country. His brother Dean, whom he has not seen since he went to college, shows up in the middle of the night and tells him their father is missing while on a hunting trip. Leaving his girlfriend behind to find their dad, Sam joins Dean in an effort to find their father in a little town called Jericho, where unmarried men disappear without a trace.', -- description - varchar(2000)
           47,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        -- duration - int
           DATEFROMPARTS(2005, 09, 14),                                                                                                                                                                                                                                                                                                                                                                                                                                                               -- release_date - datetime
           GETDATE(),                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 -- added_date - datetime
           1;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         -- owned - bit                                                                                                                                                                                  -- owned - bit
    ;

    INSERT INTO dbo.series_episode
    (
        id,
        series_id,
        episode_number,
        title,
        [description],
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT 2,                                                                                                                                                                     -- id - int
           1,                                                                                                                                                                     -- title - varchar(200)
           2,                                                                                                                                                                     -- episode_number - int
           'Wendigo',                                                                                                                                                             -- title - varchar(200)
           'Sam and Dean pose as Park Rangers to help a brother and sister search for their lost sibling, who the Winchester brothers believe may have been taken by a Wendigo.', -- description - varchar(2000)
           43,                                                                                                                                                                    -- duration - int
           DATEFROMPARTS(2005, 09, 21),                                                                                                                                           -- release_date - datetime
           GETDATE(),                                                                                                                                                             -- added_date - datetime
           1;                                                                                                                                                                     -- owned - bit                                                                                                                                                                                                                                                                                                                                      -- owned - bit
    ;

    --Act
    CREATE TABLE dbo.actual
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

    INSERT INTO dbo.actual
    (
        id,
        series_id,
        episode_number,
        title,
        [description],
        duration,
        release_date,
        added_date,
        owned
    )
    EXEC dbo.series_episode_get @id = @id, @title = NULL, @owned = NULL, @series_id = NULL;

    --Assert
    SELECT id,
           series_id,
           episode_number,
           title,
           [description],
           duration,
           release_date,
           added_date,
           owned
    INTO dbo.expected
    FROM dbo.series_episode
    WHERE id = @id;

    EXEC tSQLt.AssertEqualsTable @Expected = 'expected', @Actual = 'actual', @Message = 'Get record does not match expected record.';
END;





