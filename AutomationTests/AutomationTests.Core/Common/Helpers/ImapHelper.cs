using AutomationTests.Core.Common.Configuration;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace AutomationTests.Core.Common.Helpers;

public static class ImapHelper
{
    public static async Task<List<MimeMessage>> GetNewMessages()
    {
        var imapOptions = TestConfiguration.Imap;
        
        using var client = new ImapClient();
        await client.ConnectAsync(imapOptions.HostName, imapOptions.Port);
        await client.AuthenticateAsync(imapOptions.Email, imapOptions.Password);

        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly);

        var since = DateTime.UtcNow.AddMinutes(-5);
        var query = SearchQuery.And(SearchQuery.NotSeen, SearchQuery.DeliveredAfter(since));

        var ids = await inbox.SearchAsync(query);
        var messages = new List<MimeMessage>();
        foreach (var id in ids)
        {
            var message = await inbox.GetMessageAsync(id);

            if (message != null)
            {
                messages.Add(message);
                await inbox.AddFlagsAsync(id, MessageFlags.Seen, true);
            }
        }
        
        await client.DisconnectAsync(true);

        return messages;
    }
}