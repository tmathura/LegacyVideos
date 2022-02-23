IF NOT EXISTS (SELECT * FROM sys.schemas WHERE [name] = N'unit_tests')
    EXEC tSQLt.NewTestClass 'unit_tests';
GO