IF OBJECT_ID('[dbo].[series_get]') IS NOT NULL
	DROP PROCEDURE [dbo].[series_get];

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[series_get]
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
           release_date,
           added_date,
           ended,
           owned,
           anime
    FROM dbo.series
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
