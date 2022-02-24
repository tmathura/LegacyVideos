IF EXISTS
(
    SELECT *
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo'
          AND TABLE_NAME = 'series'
          AND COLUMN_NAME = 'announced_date'
)
BEGIN
    ALTER TABLE [dbo].[series] DROP COLUMN [announced_date];
END;