IF OBJECT_ID('[dbo].[series_episode_ins]') IS NOT NULL
    DROP PROCEDURE [dbo].[series_episode_ins];
GO

CREATE PROCEDURE [dbo].[series_episode_ins]
(
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

    INSERT INTO dbo.series_episode
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
    SELECT @series_id,      -- series_id - int
           @episode_number, -- episode_number - int
           @title,          -- title - varchar(200)
           @description,    -- description - varchar(2000)
           @duration,       -- duration - int
           @release_date,   -- release_date - datetime
           @added_date,     -- added_date - datetime
           @owned;          -- owned - bit

    SELECT SCOPE_IDENTITY();

END;
GO

