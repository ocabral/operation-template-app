SELECT TOP 1
    [modify_date] AS [LastDatabaseModificationDate],
    @@SERVERNAME as [HostName],
    @@VERSION as [Version],
    SYSDATETIME() as [CurrentDateTime]
FROM 
    sys.objects
where
    type = 'U'
ORDER BY
    [modify_date] DESC
