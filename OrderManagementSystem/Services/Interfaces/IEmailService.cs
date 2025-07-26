namespace OrderManagementSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderStatusEmailAsync(string to, string subject, string body);
    }

}
