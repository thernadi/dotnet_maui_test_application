using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ProductInventoryTestApp.Entities;

namespace ProductInventoryTestApp.InventoryData
{
    public class InventoryDbContext :  DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
