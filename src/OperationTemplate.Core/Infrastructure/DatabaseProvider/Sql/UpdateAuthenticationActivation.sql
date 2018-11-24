UPDATE
    [dbo].[Authentication]
SET
    [IsActive] = @IsActive
WHERE
    [ApplicationKey] = @ApplicationKey