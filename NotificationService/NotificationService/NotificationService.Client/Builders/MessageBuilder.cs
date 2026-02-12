using System.Text;

namespace NotificationService.Client.Builders;

public class MessageBuilder
{
    private readonly StringBuilder _content = new();

    public MessageBuilder AppendTitle(string title)
    {
        _content.Append($"<h2>{title}</h2>");
        return this;
    }

    public MessageBuilder AppendGreeting(string fullName)
    {
        AppendParagraphStart();
        _content.Append($"Hello, <strong>{fullName}</strong>!");
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
        _content.Append("<br>");
        return this;
    }
    
    public MessageBuilder AppendText(string text)
    {
        _content.Append(text);
        return this;
    }

    public MessageBuilder AppendParagraphStart()
    {
        _content.Append("<p>");
        return this;
    }
    
    public MessageBuilder AppendParagraphEnd()
    {
        _content.Append("</p>");
        return this;
    }
    
    public MessageBuilder AppendSmall(string text)
    {
        _content.Append($"<small>{text}</small>");
        return this;
    }
    
    public MessageBuilder AppendBold(string text)
    {
        _content.Append($"<b>{text}</b>");
        return this;
    }

    public string Build()
    {
        return _content.ToString();
    }

    public MessageBuilder Clear()
    {
        _content.Clear();
        return this;
    }
}
