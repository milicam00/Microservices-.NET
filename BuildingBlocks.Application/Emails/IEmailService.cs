namespace BuildingBlocks.Application.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
