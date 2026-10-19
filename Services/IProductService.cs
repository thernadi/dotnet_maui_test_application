using System;
using System.Collections.Generic;
using System.Text;
using ProductInventoryTestApp.Entities;

namespace ProductInventoryTestApp.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetProductsAsync(string? name, string? barcode, int page, int pageSize);

        public Task<Product?> GetProductAsync(int id);

        public Task<Product?> GetProductByNameAsync(string name);

        public Task<ValidationResult> ValidateProductAsync(Product product);

        public Task<ValidationResult> SaveAsync(Product product);

        public Task<int> GenerateTestDataAsync();
    }
}
