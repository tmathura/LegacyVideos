-- <Migration ID="cb7a54d9-fc58-4dae-9e1a-8a8aa5ec63e1" />
GO

PRINT N'Creating [dbo].[series]'
GO
IF OBJECT_ID(N'[dbo].[series]', 'U') IS NULL
CREATE TABLE [dbo].[series]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[title] [varchar] (200) NOT NULL,
[description] [varchar] (2000) NOT NULL,
[release_date] [datetime] NOT NULL,
[announced_date] [datetime] NOT NULL,
[added_date] [datetime] NOT NULL,
[ended] [bit] NOT NULL CONSTRAINT [DF_series_ended] DEFAULT ((0)),
[owned] [bit] NOT NULL CONSTRAINT [DF_series_owned] DEFAULT ((0))
)
GO
PRINT N'Creating primary key [PK_series_id] on [dbo].[series]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_series_id]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[series]', 'U'))
ALTER TABLE [dbo].[series] ADD CONSTRAINT [PK_series_id] PRIMARY KEY CLUSTERED ([id])
GO
