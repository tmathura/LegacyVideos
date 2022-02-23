-- <Migration ID="0201b281-978e-4e79-9deb-7273a0ba6405" />
GO

PRINT N'Altering [dbo].[series]'
GO
IF COL_LENGTH(N'[dbo].[series]', N'anime') IS NULL
ALTER TABLE [dbo].[series] ADD[anime] [bit] NOT NULL CONSTRAINT [DF_series_anime] DEFAULT ((0))
GO
