IF NOT EXISTS
(
    SELECT *
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = 'movies'
)
BEGIN
    CREATE TABLE [dbo].[movies]
    (
        [id] [INT] IDENTITY(1, 1) NOT NULL,
        [title] [VARCHAR](200) NOT NULL,
        [description] [VARCHAR](2000) NOT NULL,
        [movie_type_id] [INT] NOT NULL,
        [duration] [INT] NOT NULL,
        [release_date] [DATETIME] NOT NULL,
        [announced_date] [DATETIME] NOT NULL,
        [added_date] [DATETIME] NOT NULL,
        [owned] [BIT] NOT NULL CONSTRAINT DF_movies_owned DEFAULT 0,
        CONSTRAINT [PK_movies_id] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
    ) ON [PRIMARY];
END;
GO

IF NOT EXISTS
(
    SELECT *
    FROM sys.foreign_keys
    WHERE object_id = OBJECT_ID(N'[dbo].[FK_movies_movie_type_id]')
          AND parent_object_id = OBJECT_ID(N'[dbo].[movies]')
)
BEGIN
    ALTER TABLE dbo.[movies] WITH CHECK ADD CONSTRAINT [FK_movies_movie_type_id] FOREIGN KEY ([movie_type_id]) REFERENCES dbo.[movie_type] ([id]);
END;