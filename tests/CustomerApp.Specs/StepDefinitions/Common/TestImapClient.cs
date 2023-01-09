using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace CustomerApp.Specs.StepDefinitions.Common;

internal sealed class TestImapClient : IDisposable
{
    readonly ImapClient _imapClient;
    readonly IMailFolder _inbox;

    public TestImapClient()
    {
        _imapClient = new ImapClient();
        _imapClient.Connect("localhost", 143);
        _imapClient.Authenticate(userName: "", password: "");

        _inbox = _imapClient.Inbox;
        _inbox.Open(FolderAccess.ReadOnly);
    }

    // Search pattern seems not to work on smtp4dev => Loop manually
    internal async Task<MimeMessage?> GetFirstOrDefaultMessage(string emailAddress, int maxMessagesToFetch)
    {
        var lastMessageIndex = _inbox.Count - 1;
        foreach (var index in Enumerable.Range(0, maxMessagesToFetch))
        {
            var message = await _inbox.GetMessageAsync(lastMessageIndex - index);
            if (message.To.ToString() == emailAddress) return message;
        }
        return null;
    }

    public void Dispose()
    {
        _imapClient.Disconnect(true);
        _imapClient.Dispose();
    }
}