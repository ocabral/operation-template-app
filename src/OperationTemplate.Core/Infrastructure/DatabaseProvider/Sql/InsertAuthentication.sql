INSERT INTO [dbo].[Authentication]
(
    [ApplicationName],
	[ApplicationKey],
	[ApplicationToken],
	[IsActive],
	[CreationDateTime]
)
VALUES
(
    @ApplicationName,
	@ApplicationKey,
	@ApplicationToken,
	@IsActive,
	@CreationDateTime
);

SELECT SCOPE_IDENTITY();