IF OBJECT_ID('[dbo].[series_episode_upd]') IS NOT NULL
    DROP PROCEDURE [dbo].[series_episode_upd];
GO

CREATE PROCEDURE [dbo].[series_episode_upd]
(
    @id INT,
    @series_id INT,
    @episode_number INT,
    @title [VARCHAR](200),
    @description [VARCHAR](2000),
    @duration INT,
    @release_date [DATETIME],
    @added_date [DATETIME],
    @owned [BIT]
)
AS
BEGIN

    UPDATE dbo.series_episode
    SET series_id = @series_id,           -- title - varchar(200)
        episode_number = @episode_number, -- episode_number - int
        title = @title,                   -- title - varchar(200)
        [description] = @description,     -- description - varchar(2000)
        duration = @duration,             -- duration - int
        release_date = @release_date,     -- release_date - datetime
        added_date = @added_date,         -- added_date - datetime
        owned = @owned                    -- owned - bit
    WHERE id = @id;

END;
GO

