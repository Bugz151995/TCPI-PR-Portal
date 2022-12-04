using TCPI_PR_Portal.Data;

public interface ISendEmailService
{
    Task<bool> SendEmail(ContactDto contact);
}