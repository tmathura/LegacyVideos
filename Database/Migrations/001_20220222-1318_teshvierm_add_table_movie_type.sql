-- <Migration ID="cd3488b6-c120-458a-8dc9-7e6d390cd098" />
GO

PRINT N'Creating [dbo].[movie_type]'
GO
IF OBJECT_ID(N'[dbo].[movie_type]', 'U') IS NULL
CREATE TABLE [dbo].[movie_type]
(
[id] [int] NOT NULL IDENTITY(1, 1),
[type] [varchar] (50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_movie_type_id] on [dbo].[movie_type]'
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PK_movie_type_id]', 'PK') AND parent_object_id = OBJECT_ID(N'[dbo].[movie_type]', 'U'))
ALTER TABLE [dbo].[movie_type] ADD CONSTRAINT [PK_movie_type_id] PRIMARY KEY CLUSTERED ([id])
GO
