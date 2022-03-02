IF OBJECT_ID('[unit_tests].[test series_del test cascade delete]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_del test cascade delete];
GO

-- test that serie_episode_del deletes the series episode correctly
CREATE PROCEDURE [unit_tests].[test series_del test cascade delete]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

    DECLARE @series_id INT = 1;

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
    SELECT @series_id,                                                                                                                                                            -- id - int
           'Supernatural',                                                                                                                                                        -- title - varchar(200)
           'Two brothers follow their father''s footsteps as hunters, fighting evil supernatural beings of many kinds, including monsters, demons and gods that roam the earth.', -- description - varchar(2000)
           DATEFROMPARTS(2005, 09, 13),                                                                                                                                           -- release_date - datetime
           GETDATE(),                                                                                                                                                             -- added_date - datetime
           1,                                                                                                                                                                     -- ended - bit
           1,                                                                                                                                                                     -- owned - bit
           0                                                                                                                                                                      -- anime - bit
    ;

    EXEC tSQLt.FakeTable @TableName = 'series_episode';
    EXEC tSQLt.ApplyConstraint @TableName = 'series_episode', @ConstraintName = 'FK_series_episode_series_id', @NoCascade = 0;
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
    SELECT 1,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         -- id - int
           @series_id,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                -- title - varchar(200)
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
           @series_id,                                                                                                                                                            -- title - varchar(200)
           2,                                                                                                                                                                     -- episode_number - int
           'Wendigo',                                                                                                                                                             -- title - varchar(200)
           'Sam and Dean pose as Park Rangers to help a brother and sister search for their lost sibling, who the Winchester brothers believe may have been taken by a Wendigo.', -- description - varchar(2000)
           43,                                                                                                                                                                    -- duration - int
           DATEFROMPARTS(2005, 09, 21),                                                                                                                                           -- release_date - datetime
           GETDATE(),                                                                                                                                                             -- added_date - datetime
           1;                                                                                                                                                                     -- owned - bit                                                                                                                                                                                                                                                                                                                                      -- owned - bit
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
    SELECT 3,                                                                                                                                                                                                                                                                                                                                                         -- id - int
           @series_id,                                                                                                                                                                                                                                                                                                                                                -- title - varchar(200)
           3,                                                                                                                                                                                                                                                                                                                                                         -- episode_number - int
           'Dead in the Water',                                                                                                                                                                                                                                                                                                                                       -- title - varchar(200)
           'While going through the newspaper, Dean comes across a mysterious drowning victim. Upon further research they soon discover more people who have drowned in the same lake, but their bodies were never found. When the boys show up in town they befriend a boy whose father has drowned. The brothers come to believe the lake is haunted by a spirit.', -- description - varchar(2000)
           43,                                                                                                                                                                                                                                                                                                                                                        -- duration - int
           DATEFROMPARTS(2005, 09, 28),                                                                                                                                                                                                                                                                                                                               -- release_date - datetime
           GETDATE(),                                                                                                                                                                                                                                                                                                                                                 -- added_date - datetime
           1;                                                                                                                                                                                                                                                                                                                                                         -- owned - bit                                                                                                                                                                                  -- owned - bit
    ;

    --Act
    EXEC dbo.series_del @id = @series_id;

    --Assert
    DECLARE @seriesDeleted BIT = CASE WHEN EXISTS ((SELECT TOP (1) title FROM dbo.series WHERE id = @series_id ORDER BY id)) THEN 1 ELSE 0 END;
    DECLARE @seriesEpisodeDeleted BIT = CASE WHEN EXISTS ((SELECT TOP (1) title FROM dbo.series_episode WHERE series_id = @series_id ORDER BY id)) THEN 1 ELSE 0 END;

    EXEC tSQLt.AssertEquals @Expected = 0, @Actual = @seriesDeleted, @Message = 'Series record not deleted.';
    EXEC tSQLt.AssertEquals @Expected = 0, @Actual = @seriesEpisodeDeleted, @Message = 'Series episode record not deleted.';
END;





