using SendGrid;
using SendGrid.Helpers.Mail;
using TCPI_PR_Portal.Data;

public class SendEmailService : ISendEmailService
{
    private readonly ISendGridClient _sendGridClient;
    public SendEmailService(ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient ?? throw new ArgumentNullException(nameof(sendGridClient));
    }

    public async Task<bool> SendEmail(ContactDto contact)
    {
        SendGridMessage msg = new SendGridMessage();
        EmailAddress from = new EmailAddress("taiheyocementph@gmail.com", "Taiheiyo Cement Philippines, Inc.");
        List <EmailAddress> recipients = new List<EmailAddress> { new EmailAddress(contact.Email, contact.Name) };

        msg.SetSubject("Taiheiyo Cement Philippines");
        msg.SetFrom(from);
        msg.AddTos(recipients);
        msg.PlainTextContent = contact.Message;

        Response response = await _sendGridClient.SendEmailAsync(msg);
        if (Convert.ToInt32(response.StatusCode) >= 400)
        {
            return false;
        }
        return true;
    }
}