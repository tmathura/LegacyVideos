IF OBJECT_ID('[dbo].[movies_ins]') IS NOT NULL
    DROP PROCEDURE [dbo].[movies_ins];
GO

CREATE PROCEDURE [dbo].[movies_ins]
(
    @title [VARCHAR](200),
    @description [VARCHAR](2000),
    @movie_type_id [INT],
    @duration [INT],
    @release_date [DATETIME],
    @added_date [DATETIME],
    @owned [BIT]
)
AS
BEGIN

    INSERT INTO dbo.movies
    (
        title,
        [description],
        movie_type_id,
        duration,
        release_date,
        added_date,
        owned
    )
    SELECT @title,         -- title - varchar(200)
           @description,   -- description - varchar(2000)
           @movie_type_id, -- movie_type_id - int
           @duration,      -- duration - int
           @release_date,  -- release_date - datetime
           @added_date,    -- added_date - datetime
           @owned          -- owned - bit
    ;

    RETURN SCOPE_IDENTITY();
    
END;
GO

