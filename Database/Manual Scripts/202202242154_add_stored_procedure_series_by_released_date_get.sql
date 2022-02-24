IF OBJECT_ID('[dbo].[series_by_released_date_get]') IS NOT NULL
    DROP PROCEDURE [dbo].[series_by_released_date_get];
GO

CREATE PROCEDURE [dbo].[series_by_released_date_get]
(
    @from_date DATETIME,
    @to_date DATETIME
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
    WHERE release_date BETWEEN @from_date AND @to_date;

END;
GO

