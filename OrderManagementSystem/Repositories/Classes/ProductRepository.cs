using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Interfaces;

namespace OrderManagementSystem.Repositories.Classes
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(OrderManagementDbContext context) : base(context) { }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }
    }

}
