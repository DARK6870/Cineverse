namespace IdentityService.Mongo.Schemas.Enums;

public enum UserStatus
{
    PendingEmailConfirmation,
    Normal,
    Blocked,
    Disabled
}