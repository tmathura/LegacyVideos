IF NOT EXISTS
(
    SELECT *
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = 'series'
          AND COLUMN_NAME = 'anime'
)
BEGIN
    ALTER TABLE [dbo].[series] ADD [anime] BIT NOT NULL CONSTRAINT DF_series_anime DEFAULT 0;
END;