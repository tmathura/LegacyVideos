IF OBJECT_ID('[dbo].[movies_by_released_date_get]') IS NOT NULL
    DROP PROCEDURE [dbo].[movies_by_released_date_get];
GO

CREATE PROCEDURE [dbo].[movies_by_released_date_get]
(
    @from_date DATETIME,
    @to_date DATETIME
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
    WHERE release_date BETWEEN @from_date AND @to_date;

END;
GO

