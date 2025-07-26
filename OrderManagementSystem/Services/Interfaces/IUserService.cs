namespace OrderManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(string username, string password, string role);
        Task<string> LoginAsync(string username, string password);
    }

}
