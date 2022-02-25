IF OBJECT_ID('[dbo].[series_del]') IS NOT NULL
    DROP PROCEDURE [dbo].[series_del];
GO

CREATE PROCEDURE [dbo].[series_del]
(@id INT)
AS
BEGIN

    DELETE dbo.series
    WHERE id = @id;

END;
GO

