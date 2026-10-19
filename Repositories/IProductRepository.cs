using ProductInventoryTestApp.Entities;

namespace ProductInventoryTestApp.Repositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetProductsAsync(string name, string barcode, int page = 1, int pageSize = 10);
        public Task<Product?> GetProductAsync(int id);

        public Task<Product?> GetProductByNameAsync(string name);

        public Task<Product?> GetProductByBarcodeAsync(string barcode);

        public Task SaveAsync(Product product);

        public Task<int> GenerateTestDataAsync();
    }
}