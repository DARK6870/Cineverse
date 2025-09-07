using System.Text;

namespace Cineverse.Notifications.Common.Builders;

public class MessageBuilder
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    public MessageBuilder AppendTitle(string title)
    {
        _stringBuilder.Append($"<h2>{title}</h2>");
        return this;
    }

    public MessageBuilder AppendGreeting(string fullName)
    {
        _stringBuilder.Append($"<p>Hello, <strong>{fullName}</strong>!</p>");
        return this;
    }

    public MessageBuilder AppendMessage(string message)
    {
        _stringBuilder.Append($"<p>{message}</p><br><br>");
        return this;
    }

    public MessageBuilder AppendAction(string url, string actionText)
    {
        _stringBuilder.Append($"<p><small><a href=\"{url}\">Click here</a> {actionText}.<small></p>");
        return this;
    }

    public MessageBuilder AppendLineBreak()
    {
        _stringBuilder.Append("<br>");
        return this;
    }

    public MessageBuilder AppendParagraph(string text)
    {
        _stringBuilder.Append($"<p>{text}</p>");
        return this;
    }
    
    public MessageBuilder AppendBold(string text)
    {
        _stringBuilder.Append($"<b>{text}</b>");
        return this;
    }
    
    public MessageBuilder AppendSmall(string text)
    {
        _stringBuilder.Append($"<small>{text}</small>");
        return this;
    }

    public string Build()
    {
        return _stringBuilder.ToString();
    }

    public MessageBuilder Clear()
    {
        _stringBuilder.Clear();
        return this;
    }
}
