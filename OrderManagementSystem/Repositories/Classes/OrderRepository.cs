using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(OrderManagementDbContext context) : base(context) { }

    public async Task<IEnumerable<Order>> GetAllWithItemsAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }
}
