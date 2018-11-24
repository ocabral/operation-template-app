SELECT
    [Id],
    [ApplicationName],
	[ApplicationKey],
    [ApplicationToken],
	[IsActive],
    [CreationDateTime]
FROM
    [dbo].[Authentication] aut
WHERE
    1=1
#WHERE
ORDER BY
    aut.[Id]
#PAGING