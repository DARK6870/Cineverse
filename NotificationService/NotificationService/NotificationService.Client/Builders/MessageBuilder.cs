using System.Text;

namespace NotificationService.Client.Builders;

public class MessageBuilder
{
    private readonly StringBuilder _stringBuilder = new();

    public MessageBuilder AppendTitle(string title)
    {
        _stringBuilder.Append($"<h2>{title}</h2>");
        return this;
    }

    public MessageBuilder AppendGreeting(string fullName)
    {
        AppendParagraphStart();
        _stringBuilder.Append($"Hello, <strong>{fullName}</strong>!");
        AppendParagraphEnd();
        
        return this;
    }

    public MessageBuilder AppendAction(string url, string actionText)
    {
        AppendLineBreak();
        AppendLineBreak();
        AppendLineBreak();
        AppendParagraphStart();
        AppendSmall($"<a href=\"{url}\">Click here</a> {actionText}");
        AppendParagraphEnd();
        
        return this;
    }

    public MessageBuilder AppendLineBreak()
    {
        _stringBuilder.Append("<br>");
        return this;
    }
    
    public MessageBuilder AppendText(string text)
    {
        _stringBuilder.Append(text);
        return this;
    }

    public MessageBuilder AppendParagraphStart()
    {
        _stringBuilder.Append("<p>");
        return this;
    }
    
    public MessageBuilder AppendParagraphEnd()
    {
        _stringBuilder.Append("</p>");
        return this;
    }
    
    public MessageBuilder AppendSmall(string text)
    {
        _stringBuilder.Append($"<small>{text}</small>");
        return this;
    }
    
    public MessageBuilder AppendBold(string text)
    {
        _stringBuilder.Append($"<b>{text}</b>");
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
