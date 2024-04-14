using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : GenericRepository<Product, AppProductContext>, IProductRepository<AppProductContext>
    {
        private readonly DbContext dbContext;

        public ProductRepository(AppProductContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<bool> CategoryCheck(int categoryId)
        {
            return await dbContext.Set<Product>().Where(s => s.CategoryId == categoryId).Include(s => s.Category).AnyAsync();
        }
    }
}