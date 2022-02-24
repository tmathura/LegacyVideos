IF NOT EXISTS
(
    SELECT *
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = 'series_episode'
)
BEGIN
    CREATE TABLE [dbo].[series_episode]
    (
        [id] [INT] IDENTITY(1, 1) NOT NULL,
        [series_id] [INT] NOT NULL,
        [episode_number] [INT] NOT NULL,
        [title] [VARCHAR] (200) NOT NULL,
        [description] [VARCHAR](2000) NOT NULL,
        [duration] [INT] NOT NULL,
        [release_date] [DATETIME] NOT NULL,
        [added_date] [DATETIME] NOT NULL,
        [owned] [BIT] NOT NULL CONSTRAINT DF_series_episode_owned DEFAULT 0,
        CONSTRAINT [PK_series_episode_id] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY],
		CONSTRAINT [IX_series_id_episode_number] UNIQUE NONCLUSTERED
		(
		[series_id], [episode_number] ASC
		) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY];
END;
GO

IF NOT EXISTS
(
    SELECT *
    FROM sys.foreign_keys
    WHERE object_id = OBJECT_ID(N'[dbo].[FK_series_episode_series_id]')
          AND parent_object_id = OBJECT_ID(N'[dbo].[series_episode]')
)
BEGIN
    ALTER TABLE dbo.[series_episode] WITH CHECK
    ADD CONSTRAINT [FK_series_episode_series_id] FOREIGN KEY ([series_id]) REFERENCES dbo.[series] ([id]);
END;