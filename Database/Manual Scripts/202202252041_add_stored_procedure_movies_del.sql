IF OBJECT_ID('[dbo].[movies_del]') IS NOT NULL
    DROP PROCEDURE [dbo].[movies_del];
GO

CREATE PROCEDURE [dbo].[movies_del]
(@id INT)
AS
BEGIN

    DELETE dbo.movies
    WHERE id = @id;

END;
GO

