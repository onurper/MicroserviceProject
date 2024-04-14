using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppProductInvoiceContext : DbContext
    {
        public AppProductInvoiceContext(DbContextOptions<AppProductInvoiceContext> options) : base(options)

        {
        }

        public DbSet<ProductInvoice> ProductInvoices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
        }
    }
}