using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int customerId, List<(int productId, int quantity)> items, string paymentMethod);
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
    }

}
