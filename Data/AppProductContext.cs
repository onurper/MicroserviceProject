using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppProductContext : DbContext
    {
        public AppProductContext(DbContextOptions<AppProductContext> options) : base(options)

        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
        }
    }
}