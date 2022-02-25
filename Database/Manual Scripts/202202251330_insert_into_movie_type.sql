SET IDENTITY_INSERT dbo.movie_type ON;

IF NOT EXISTS (SELECT 1 FROM [dbo].[movie_type] WHERE id = 1)
BEGIN

    INSERT INTO dbo.movie_type
    (
        id,
        [type]
    )
    SELECT 1, 'Dvd';
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[movie_type] WHERE id = 2)
BEGIN

    INSERT INTO dbo.movie_type
    (
        id,
        [type]
    )
    SELECT 2, 'BluRay';
END;

SET IDENTITY_INSERT dbo.movie_type OFF;