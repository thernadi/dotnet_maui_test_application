using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductInventoryTestApp.Entities;
using ProductInventoryTestApp.Pages;
using ProductInventoryTestApp.Services;
using System.Collections.ObjectModel;

namespace ProductInventoryTestApp.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; set; }
            = new();

        [ObservableProperty]
        private Product selectedProduct;

        private int? _selectedProductId;

        [ObservableProperty]
        private string? filterName;
        [ObservableProperty]
        private string? filterBarcode;
        [ObservableProperty]
        private int currentPage = 1;
        [ObservableProperty]
        private int pageSize = 50;

        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public async Task LoadProductsAsync()
        {

            var products = await _productService.GetProductsAsync(
                name: FilterName,
                barcode: FilterBarcode,
                page: CurrentPage,
                pageSize: PageSize);

            Products.Clear();

            foreach (var product in products)
                Products.Add(product);

            if (_selectedProductId.HasValue)
            {
                SelectedProduct = Products
                    .FirstOrDefault(p => p.Id == _selectedProductId.Value);
            }

        }

        [RelayCommand]
        private async Task FilterAsync()
        {
            CurrentPage = 1;
            await LoadProductsAsync();
        }

        [RelayCommand]
        private async Task PreviousAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadProductsAsync();
            }
        }

        [RelayCommand]
        private async Task NextAsync()
        {
            CurrentPage++;
            await LoadProductsAsync();
            if (Products.Count == 0 && CurrentPage > 1)
            {
                CurrentPage--;
                await LoadProductsAsync();
            }              
        }

        [RelayCommand]
        private async Task CleanFilterAsync()
        {
            FilterName = null;
            FilterBarcode = null;
        }

        [RelayCommand]
        private async Task AddNewProductAsync()
        {
            await Shell.Current.GoToAsync(nameof(ProductPage),
            new Dictionary<string, object>
            {
                ["SelectedProduct"] = new Product()
            });
        }

        [RelayCommand]
        private async Task ModifyProductAsync()
        {
            if (SelectedProduct == null)
                return;

            _selectedProductId = SelectedProduct.Id;

            await Shell.Current.GoToAsync(nameof(ProductPage),
                new Dictionary<string, object>
                {
                    ["SelectedProduct"] = SelectedProduct
                });
        }
    }
}