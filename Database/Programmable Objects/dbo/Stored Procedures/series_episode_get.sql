IF OBJECT_ID('[dbo].[series_episode_get]') IS NOT NULL
	DROP PROCEDURE [dbo].[series_episode_get];

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[series_episode_get]
(
    @id INT,
    @title [VARCHAR](200),
    @owned BIT,
    @series_id INT
)
AS
BEGIN

    SELECT id,
           series_id,
           episode_number,
           title,
           [description],
           duration,
           release_date,
           added_date,
           owned
    FROM dbo.series_episode
    WHERE (
              id = @id
              OR @id IS NULL
          )
          AND
          (
              title LIKE '%' + @title + '%'
              OR @title IS NULL
          )
          AND
          (
              owned = @owned
              OR @owned IS NULL
          )
          AND
          (
              series_id = @series_id
              OR @series_id IS NULL
          );

END;
GO
