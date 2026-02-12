namespace IdentityService.Application.Helpers;

public static class EmailVerificationHelper
{
    private static readonly Random Random = new Random();
    
    public static int GenerateVerificationCode()
    {
        return Random.Next(11111, 99999);
    }
}