IF OBJECT_ID('[unit_tests].[test series_episode_upd valid update]') IS NOT NULL
    DROP PROCEDURE [unit_tests].[test series_episode_upd valid update];
GO

-- test that serie_episodes_upd updates the series episode correctly
CREATE PROCEDURE [unit_tests].[test series_episode_upd valid update]
AS
BEGIN
    --Assemble
    IF OBJECT_ID('actual') IS NOT NULL DROP TABLE dbo.actual;
    IF OBJECT_ID('expected') IS NOT NULL DROP TABLE dbo.expected;

    DECLARE @id INT = 2;
    DECLARE @series_id INT = 1;
    DECLARE @episode_number INT = 2;
    DECLARE @wrong_title [VARCHAR](200) = 'Wendigos';
    DECLARE @correct_title [VARCHAR](200) = 'Wendigo';
    DECLARE @description [VARCHAR](2000) = 'Sam and Dean pose as Park Rangers to help a brother and sister search for their lost sibling, who the Winchester brothers believe may have been taken by a Wendigo.';
    DECLARE @duration INT = 43;
    DECLARE @release_date [DATETIME] = DATEFROMPARTS(2005, 09, 21);
    DECLARE @added_date [DATETIME] = GETDATE();
    DECLARE @owned [BIT] = 1;

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
    SELECT 1,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         -- id - int
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
    SELECT @id,             -- id - int
           @series_id,      -- title - varchar(200)
           @episode_number, -- episode_number - int
           @wrong_title,    -- title - varchar(200)
           @description,    -- description - varchar(2000)
           @duration,       -- duration - int
           @release_date,   -- release_date - datetime
           @added_date,     -- added_date - datetime
           @owned;          -- owned - bit                                                                                                                                                                                                                                                                                                                                      -- owned - bit
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
           1,                                                                                                                                                                                                                                                                                                                                                         -- title - varchar(200)
           3,                                                                                                                                                                                                                                                                                                                                                         -- episode_number - int
           'Dead in the Water',                                                                                                                                                                                                                                                                                                                                       -- title - varchar(200)
           'While going through the newspaper, Dean comes across a mysterious drowning victim. Upon further research they soon discover more people who have drowned in the same lake, but their bodies were never found. When the boys show up in town they befriend a boy whose father has drowned. The brothers come to believe the lake is haunted by a spirit.', -- description - varchar(2000)
           43,                                                                                                                                                                                                                                                                                                                                                        -- duration - int
           DATEFROMPARTS(2005, 09, 28),                                                                                                                                                                                                                                                                                                                               -- release_date - datetime
           GETDATE(),                                                                                                                                                                                                                                                                                                                                                 -- added_date - datetime
           1;                                                                                                                                                                                                                                                                                                                                                         -- owned - bit                                                                                                                                                                                  -- owned - bit
    ;

    --Act
    EXEC dbo.series_episode_upd @id = @id,
                                @series_id = @series_id,
                                @episode_number = @episode_number,
                                @title = @correct_title,
                                @description = @description,
                                @duration = @duration,
                                @release_date = @release_date,
                                @added_date = @added_date,
                                @owned = @owned;

    --Assert
    DECLARE @title [VARCHAR](200) = (SELECT TOP (1) title FROM dbo.series_episode WHERE id = @id ORDER BY id);

    EXEC tSQLt.AssertEquals @Expected = @title, @Actual = @correct_title, @Message = 'Update duration value not correct.';
END;





