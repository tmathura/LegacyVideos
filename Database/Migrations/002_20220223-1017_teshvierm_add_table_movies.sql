-- <Migration ID="aa16b2b4-ba53-4fb7-bff6-07383e9354c4" />
GO

PRINT N'Creating [dbo].[movies]'
GO
IF OBJECT_ID(N'[dbo].[movies]', 'U') IS NULL
CREATE TABLE [dbo].[movies]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[title] [varchar] (200) NOT NULL,
[description] [varchar] (2000) NOT NULL,
[movie_type_id] [int] NOT NULL,
[duration] [int] NOT NULL,
[release_date] [datetime] NOT NULL,
[announced_date] [datetime] NOT NULL,
[added_date] [datetime] NOT NULL,
[owned] [bit] NOT NULL CONSTRAINT [DF_movies_owned] DEFAULT ((0))
)
GO
PRINT N'Creating primary key [PK_movies_id] on [dbo].[movies]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_movies_id]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[movies]', 'U'))
ALTER TABLE [dbo].[movies] ADD CONSTRAINT [PK_movies_id] PRIMARY KEY CLUSTERED ([id])
GO
PRINT N'Adding foreign keys to [dbo].[movies]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_movies_movie_type_id]','F') AND parent_object_id = OBJECT_ID(N'[dbo].[movies]', 'U'))
ALTER TABLE [dbo].[movies] ADD CONSTRAINT [FK_movies_movie_type_id] FOREIGN KEY ([movie_type_id]) REFERENCES [dbo].[movie_type] ([id])
GO
