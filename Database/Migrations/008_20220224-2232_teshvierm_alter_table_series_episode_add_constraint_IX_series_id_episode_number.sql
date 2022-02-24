-- <Migration ID="d0b4d9f2-095d-4731-8789-0f9916eb8ae7" />
GO

PRINT N'Adding constraints to [dbo].[series_episode]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IX_series_id_episode_number]', 'UQ') AND parent_object_id = OBJECT_ID(N'[dbo].[series_episode]', 'U'))
ALTER TABLE [dbo].[series_episode] ADD CONSTRAINT [IX_series_id_episode_number] UNIQUE NONCLUSTERED ([series_id], [episode_number])
GO
