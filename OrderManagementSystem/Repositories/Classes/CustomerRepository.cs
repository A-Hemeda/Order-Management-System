using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(OrderManagementDbContext context) : base(context) { }

    public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }
}
