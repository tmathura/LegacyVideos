-- <Migration ID="af9d661c-4e0a-4c2b-9d8f-a5b1fe0ae5d7" />
GO


SET DATEFORMAT YMD;


GO
IF (SELECT COUNT(*)
    FROM   [dbo].[movie_type]) = 0
    BEGIN
        PRINT (N'Add 2 rows to [dbo].[movie_type]');
        SET IDENTITY_INSERT [dbo].[movie_type] ON;
        INSERT  INTO [dbo].[movie_type] ([id], [type])
        VALUES                         (1, 'Dvd');
        INSERT  INTO [dbo].[movie_type] ([id], [type])
        VALUES                         (2, 'BluRay');
        SET IDENTITY_INSERT [dbo].[movie_type] OFF;
    END


GO