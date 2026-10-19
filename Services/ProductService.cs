using System;
using System.Collections.Generic;
using System.Text;
using ProductInventoryTestApp.Entities;
using ProductInventoryTestApp.Repositories;

namespace ProductInventoryTestApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task<List<Product>> GetProductsAsync(string name, string barcode, int page, int pageSize)
        {
            return _productRepository.GetProductsAsync(name, barcode, page, pageSize);
        }
        public Task<Product?> GetProductAsync(int id)
        {
            return _productRepository.GetProductAsync(id);
        } 

        public async Task<ValidationResult> SaveAsync(Product product)
        {
            ValidationResult validation = await ValidateProductAsync(product);
            if (!validation.Success)
            {
                return validation;
            }

            await _productRepository.SaveAsync(product);
            return new ValidationResult
            {
                Success = true
            };
        }

        public Task<int> GenerateTestDataAsync()
        {
            return _productRepository.GenerateTestDataAsync();
        }

        public async Task<ValidationResult> ValidateProductAsync(Product product)
        {
            ValidationResult validation = new ValidationResult();

            if (string.IsNullOrEmpty(product.Name))
                validation.ErrorMessages.Add("A termék neve nem lehet üres!");
            else if (product.Name.Length > 100)
                validation.ErrorMessages.Add("A termék neve nem lehet hosszabb 100 karakternél!");
            else if (product.Name.Length < 3)
                validation.ErrorMessages.Add("A termék neve nem lehet rövidebb 3 karakternél!");
            else
            { 
                Product? existingProduct = await GetProductByNameAsync(product.Name);
                if (existingProduct != null && existingProduct.Id != product.Id)
                {
                    validation.ErrorMessages.Add("A termék létezik név alapján!");
                }
            }
            if (!string.IsNullOrEmpty(product.Barcode))
            {
                if (product.Barcode.Length > 50)
                    validation.ErrorMessages.Add("A termék vonalkódja nem lehet hosszabb 50 karakternél!");
                else if (product.Barcode.Length < 3)
                    validation.ErrorMessages.Add("A termék vonalkódja nem lehet rövidebb 3 karakternél!");
                else
                {
                    Product? existingProduct = await GetProductByBarcodeAsync(product.Barcode);
                    if (existingProduct != null && existingProduct.Id != product.Id)
                    {
                        validation.ErrorMessages.Add("A termék létezik vonalkód alapján!");
                    }
                }
            }

            if (product.Price != null
                && product.Price < 0)
            {
                validation.ErrorMessages.Add("Érvénytelen ár!");
            }
            if (product.Quantity != null
                && product.Quantity < 0)
            {
                validation.ErrorMessages.Add("Érvénytelen mennyiség!");
            }

            validation.Success = validation.ErrorMessages.Count == 0;
            return validation;
        }

        public Task<Product?> GetProductByNameAsync(string name)
        {
            return _productRepository.GetProductByNameAsync(name);
        }

        public Task<Product?> GetProductByBarcodeAsync(string barcode)
        {
            return _productRepository.GetProductByBarcodeAsync(barcode);
        }
    }


    public class ValidationResult
    {
        public bool Success { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
