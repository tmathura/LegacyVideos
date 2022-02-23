IF NOT EXISTS
(
    SELECT *
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = 'series'
)
BEGIN
    CREATE TABLE [dbo].[series]
    (
        [id] [INT] IDENTITY(1, 1) NOT NULL,
        [title] [VARCHAR](200) NOT NULL,
        [description] [VARCHAR](2000) NOT NULL,
        [release_date] [DATETIME] NOT NULL,
        [announced_date] [DATETIME] NOT NULL,
        [added_date] [DATETIME] NOT NULL,
        [ended] [BIT] NOT NULL CONSTRAINT DF_series_ended DEFAULT 0,
        [owned] [BIT] NOT NULL CONSTRAINT DF_series_owned DEFAULT 0,
        CONSTRAINT [PK_series_id] PRIMARY KEY CLUSTERED ([id] ASC) ON [PRIMARY]
    ) ON [PRIMARY];
END;
GO