IF OBJECT_ID('[dbo].[series_ins]') IS NOT NULL
	DROP PROCEDURE [dbo].[series_ins];

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].[series_ins]
(
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

    INSERT INTO dbo.series
    (
        title,
        [description],
        release_date,
        added_date,
        ended,
        owned,
        anime
    )
    SELECT @title,        -- title - varchar(200)
           @description,  -- description - varchar(2000)
           @release_date, -- release_date - datetime
           @added_date,   -- added_date - datetime
           @ended,        -- ended - bit
           @owned,        -- owned - bit
           @anime;        -- anime - bit

    SELECT SCOPE_IDENTITY();

END;
GO
