USE master;
GO

DECLARE @databaseName VARCHAR(75);

SET @databaseName = 'LegacyVideos';

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = @databaseName)
BEGIN
    DECLARE @sqlCommand NVARCHAR(1000);
    SET @sqlCommand = N'CREATE DATABASE [' + @databaseName + N']';

    EXECUTE sp_executesql @sqlCommand;
END;