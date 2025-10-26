namespace Cineverse.Notifications.Common.Options;

public class NotificationLinksOptions
{
    public required string BaseUrl { get; init; }
    public required string ConfirmEmailPath { get; init; }
    public required string BookingDetailsPath { get; init; }
    public required string ProfilePath { get; init; }
    
    public required string RestorePasswordPath { get; init; }


    public string BuildConfirmEmailUrl(int verificationCode)
    {
        return BaseUrl + ConfirmEmailPath.Replace("code", verificationCode.ToString());
    }

    public string BuildBookingDetailsUrl(string bookingId)
    {
        return BaseUrl + BookingDetailsPath.Replace("id", bookingId);
    }

    public string BuildProfileUrl()
    {
        return BaseUrl + ProfilePath;
    }
    
    public string BuildRestorePasswordUrl(string email, string code)
    {
        return BaseUrl + RestorePasswordPath.Replace("email", email).Replace("code", code);
    }
}