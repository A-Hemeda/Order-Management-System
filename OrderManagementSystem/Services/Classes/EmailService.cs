using OrderManagementSystem.Services.Interfaces;

namespace OrderManagementSystem.Services.Classes
{
    public class EmailService : IEmailService
    {
        public Task SendOrderStatusEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine(" Email Sent:");
            Console.WriteLine($"To: {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body:\n{body}");
            Console.WriteLine("----------------------------------");

            return Task.CompletedTask;
        }
    }

}
