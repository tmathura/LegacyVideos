-- <Migration ID="8702e3b1-ec6c-40fc-bd5d-ea9587dd23d2" />
GO

PRINT N'Creating [dbo].[series_episode]'
GO
IF OBJECT_ID(N'[dbo].[series_episode]', 'U') IS NULL
CREATE TABLE [dbo].[series_episode]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[series_id] [int] NOT NULL,
[episode_number] [int] NOT NULL,
[title] [varchar] (200) NOT NULL,
[description] [varchar] (2000) NOT NULL,
[duration] [int] NOT NULL,
[release_date] [datetime] NOT NULL,
[added_date] [datetime] NOT NULL,
[owned] [bit] NOT NULL CONSTRAINT [DF_series_episode_owned] DEFAULT ((0))
)
GO
PRINT N'Creating primary key [PK_series_episode_id] on [dbo].[series_episode]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_series_episode_id]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[series_episode]', 'U'))
ALTER TABLE [dbo].[series_episode] ADD CONSTRAINT [PK_series_episode_id] PRIMARY KEY CLUSTERED ([id])
GO
PRINT N'Adding foreign keys to [dbo].[series_episode]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_series_episode_series_id]','F') AND parent_object_id = OBJECT_ID(N'[dbo].[series_episode]', 'U'))
ALTER TABLE [dbo].[series_episode] ADD CONSTRAINT [FK_series_episode_series_id] FOREIGN KEY ([series_id]) REFERENCES [dbo].[series] ([id]) ON DELETE CASCADE
GO
