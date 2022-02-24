-- <Migration ID="52864dc6-4c56-4e96-8947-874291ae5643" />
GO

PRINT N'Creating index [IX_movies_release_date] on [dbo].[movies]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_movies_release_date' AND object_id = OBJECT_ID(N'[dbo].[movies]'))
CREATE NONCLUSTERED INDEX [IX_movies_release_date] ON [dbo].[movies] ([release_date])
GO
