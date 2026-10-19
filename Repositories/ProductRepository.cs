using Microsoft.EntityFrameworkCore;
using ProductInventoryTestApp.Entities;
using ProductInventoryTestApp.InventoryData;
using ProductInventoryTestApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductInventoryTestApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;

        public ProductRepository(InventoryDbContext context)
        {
            _context = context;
        }

        private IQueryable<Product> ProductQuery()
        {
            return _context.Products
                .AsNoTracking()
                .AsSplitQuery().OrderBy(o => o.Id);
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await ProductQuery()
            .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<Product>> GetProductsAsync(string name, string barcode, int page = 1, int pageSize = 10)
        {
            List<Product> returnValue = new List<Product>();
            if (!string.IsNullOrEmpty(name) 
                && !string.IsNullOrEmpty(barcode))
            {
                returnValue = await ProductQuery()
                    .Where(w => w.Name.Contains(name) 
                    && !string.IsNullOrEmpty(w.Barcode)
                    && w.Barcode.Contains(barcode))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else if (!string.IsNullOrEmpty(name))
            {
                returnValue = await ProductQuery()
                    .Where(w => w.Name.Contains(name))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else if (!string.IsNullOrEmpty(barcode))
            {
                returnValue = await ProductQuery()
                .Where(w => !string.IsNullOrEmpty(w.Barcode)
                && w.Barcode.Contains(barcode))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            else
                returnValue = await ProductQuery()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            return returnValue;
        }

        public async Task SaveAsync(Product product)
        {
            if (product.Id == 0)
            {
                await _context.Products.AddAsync(product);
            }
            else
            {
                _context.Products.Update(product);
            }

            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        public async Task<int> GenerateTestDataAsync()
        {            
            Task<int> returnValue = Task.FromResult(0);
            await _context.Database.EnsureCreatedAsync();

            if (!await _context.Products.AnyAsync()) //TEST DATA GENERATION
            {
                for (int i = 0; i < 2000; i++)
                {
                    _context.Products.Add(new Product()
                    {
                        Name = $"Product{i}",
                        Barcode = $"599000{i:0000000}",
                        Price = Random.Shared.Next(100, 50000),
                        Quantity = Random.Shared.Next(0, 100)
                    });
                }
                returnValue =  _context.SaveChangesAsync();
            }

            return await returnValue;
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await ProductQuery()
                .FirstOrDefaultAsync(f => f.Name == name);
        }

        public async Task<Product?> GetProductByBarcodeAsync(string barcode)
        {
            return await ProductQuery()
                .FirstOrDefaultAsync(f => f.Barcode == barcode);
        }
    }
}
