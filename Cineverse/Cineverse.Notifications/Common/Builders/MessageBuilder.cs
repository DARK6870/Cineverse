using System.Text;

namespace Cineverse.Notifications.Common.Builders;

public class MessageBuilder
{
    public string? Title { get; set; }
    public string? FullName { get; set; }
    public required string Message { get; set; }
    public string? ActionUrl { get; set; }
    public string? ActionText { get; set; }

    
    public string FormattedMessage
    {
        get
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<h2>{Title}</h2>");
            
            if (FullName is not null)
                stringBuilder.Append($"<p>Hello, <strong>{FullName}</strong>!</p>");
            
            stringBuilder.Append($"<p>{Message}</p><br><br>");

            if (!string.IsNullOrEmpty(ActionUrl))
                stringBuilder.Append($"<p><small><a href=\"{ActionUrl}\">Click here</a> {ActionText}.<small></p>");

            return stringBuilder.ToString();
        }
    }
}