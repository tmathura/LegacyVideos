-- <Migration ID="45f3718c-8c2f-4f68-a21b-8af86cc2621a" />
GO

PRINT N'Creating index [IX_series_episode_release_date] on [dbo].[series_episode]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_series_episode_release_date' AND object_id = OBJECT_ID(N'[dbo].[series_episode]'))
CREATE NONCLUSTERED INDEX [IX_series_episode_release_date] ON [dbo].[series_episode] ([release_date])
GO
