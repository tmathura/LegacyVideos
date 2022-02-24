IF OBJECT_ID('[dbo].[movies_upd]') IS NOT NULL
    DROP PROCEDURE [dbo].[movies_upd];
GO

CREATE PROCEDURE [dbo].[movies_upd]
(
    @id INT,
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

    UPDATE dbo.movies
    SET title = @title,                 -- title - varchar(200)
        [description] = @description,     -- description - varchar(2000)
        movie_type_id = @movie_type_id, -- movie_type_id - int
        duration = @duration,           -- duration - int
        release_date = @release_date,   -- release_date - datetime
        added_date = @added_date,       -- added_date - datetime
        owned = @owned                  -- owned - bit
    WHERE id = @id;

END;
GO

