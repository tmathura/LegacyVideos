EXEC sp_configure 'show advanced options', '1';
RECONFIGURE;
EXEC sp_configure 'xp_cmdshell', '1';
RECONFIGURE;
DECLARE @BasePath VARCHAR(8000);
SET @BasePath = 'C:\Development\Private\LegacyVideos\Database\Unit Tests';
IF OBJECT_ID('tempdb..#DirectoryTree') IS NOT NULL
BEGIN
    DROP TABLE #DirectoryTree;
END;
CREATE TABLE #DirectoryTree
(
    id INT IDENTITY(1, 1),
    fullpath VARCHAR(2000) NULL,
    subdirectory NVARCHAR(512) NULL,
    depth INT NULL,
    isfile BIT NULL
);
INSERT #DirectoryTree
(
    subdirectory,
    depth,
    isfile
)
EXEC master.sys.xp_dirtree @BasePath, 1, 1;
DECLARE @fileName VARCHAR(MAX);
DECLARE fileList CURSOR FOR(
SELECT subdirectory
FROM #DirectoryTree
WHERE isfile = 1);
OPEN fileList;
FETCH NEXT FROM fileList
INTO @fileName;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @fileName = @BasePath + '\' + @fileName;
    PRINT @fileName;
    DECLARE @sqlCmd VARCHAR(4000);
    SET @sqlCmd = 'master.dbo.xp_cmdshell ''sqlcmd -S localhost -d LegacyVideos -i "' + @fileName + '"''';
    EXECUTE (@sqlCmd);
    FETCH NEXT FROM fileList
    INTO @fileName;
END;
CLOSE fileList;
DEALLOCATE fileList;
IF OBJECT_ID('tempdb..#DirectoryTree') IS NOT NULL
BEGIN
    DROP TABLE #DirectoryTree;
END;
EXEC sp_configure 'show advanced options', '1';
RECONFIGURE;
EXEC sp_configure 'xp_cmdshell', '0';
RECONFIGURE;