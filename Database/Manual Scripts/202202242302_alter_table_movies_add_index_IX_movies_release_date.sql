IF NOT EXISTS
(
    SELECT *
    FROM sys.indexes
    WHERE name = 'IX_movies_release_date'
          AND object_id = OBJECT_ID('dbo.movies')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_movies_release_date ON dbo.movies (release_date);
END;