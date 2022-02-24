IF OBJECT_ID('[dbo].[series_episode_ins]') IS NOT NULL
	DROP PROCEDURE [dbo].[series_episode_ins];

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[series_episode_ins]
(
    @series_id INT,
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
        [description],
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT @series_id,    -- series_id - int
           @description,  -- description - varchar(2000)
           @duration,     -- duration - int
           @release_date, -- release_date - datetime
           @added_date,   -- added_date - datetime
           @owned;        -- owned - bit

    RETURN SCOPE_IDENTITY();

END;
GO
