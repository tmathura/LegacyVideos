IF NOT EXISTS
(
    SELECT *
    FROM sys.indexes
    WHERE name = 'IX_series_episode_release_date'
          AND object_id = OBJECT_ID('dbo.series_episode')
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_series_episode_release_date ON dbo.series_episode (release_date);
END;