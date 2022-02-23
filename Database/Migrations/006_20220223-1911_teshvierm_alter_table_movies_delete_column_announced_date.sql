-- <Migration ID="fd925be1-76e8-4f4a-aa06-99c15307af38" />
GO

PRINT N'Altering [dbo].[movies]'
GO
IF COL_LENGTH(N'[dbo].[movies]', N'announced_date') IS NOT NULL
ALTER TABLE [dbo].[movies] DROP COLUMN [announced_date]
GO
