IF OBJECT_ID('[dbo].[movies_get]') IS NOT NULL
	DROP PROCEDURE [dbo].[movies_get];

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[movies_get]
(
    @id INT,
    @title [VARCHAR](200),
    @owned BIT
)
AS
BEGIN

    SELECT id,
           title,
           [description],
           movie_type_id,
           duration,
           release_date,
           added_date,
           owned
    FROM dbo.movies
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
          );

END;
GO
