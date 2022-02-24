IF NOT EXISTS
(
    SELECT *
    FROM sys.indexes
    WHERE name = 'IX_series_release_date'
          AND object_id = OBJECT_ID('dbo.series')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_series_release_date ON dbo.series (release_date);
END;