-- <Migration ID="d6b50aea-f499-483a-92d2-1cf49e31ec8f" />
GO

PRINT N'Altering [dbo].[series]'
GO
IF COL_LENGTH(N'[dbo].[series]', N'announced_date') IS NOT NULL
ALTER TABLE [dbo].[series] DROP COLUMN [announced_date]
GO
