IF OBJECT_ID('[dbo].[series_episode_del]') IS NOT NULL
	DROP PROCEDURE [dbo].[series_episode_del];

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[series_episode_del]
(@id INT)
AS
BEGIN

    DELETE dbo.series_episode
    WHERE id = @id;

END;
GO
