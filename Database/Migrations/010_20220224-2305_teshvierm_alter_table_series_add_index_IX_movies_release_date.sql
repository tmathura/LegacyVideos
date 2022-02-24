-- <Migration ID="e6a98f09-76d7-4e51-bbdf-112010ec05a7" />
GO

PRINT N'Creating index [IX_series_release_date] on [dbo].[series]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_series_release_date' AND object_id = OBJECT_ID(N'[dbo].[series]'))
CREATE NONCLUSTERED INDEX [IX_series_release_date] ON [dbo].[series] ([release_date])
GO
