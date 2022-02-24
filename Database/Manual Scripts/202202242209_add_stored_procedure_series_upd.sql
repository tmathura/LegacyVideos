IF OBJECT_ID('[dbo].[series_upd]') IS NOT NULL
    DROP PROCEDURE [dbo].[series_upd];
GO

CREATE PROCEDURE [dbo].[series_upd]
(
    @id INT,
    @title [VARCHAR](200),
    @description [VARCHAR](2000),
    @release_date [DATETIME],
    @added_date [DATETIME],
    @ended [BIT],
    @owned [BIT],
    @anime [BIT]
)
AS
BEGIN

    UPDATE dbo.series
    SET title = @title,               -- title - varchar(200)
        [description] = @description, -- description - varchar(2000)
        release_date = @release_date, -- release_date - datetime
        added_date = @added_date,     -- added_date - datetime
        ended = @ended,               -- ended - bit
        owned = @owned,               -- owned - bit
        anime = @anime                -- anime - bit
    WHERE id = @id;

END;
GO

