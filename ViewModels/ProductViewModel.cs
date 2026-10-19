using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProductInventoryTestApp.Entities;
using ProductInventoryTestApp.Pages;
using ProductInventoryTestApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ProductInventoryTestApp.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private List<string> errorMessages;


        [ObservableProperty]
        private Product? currentProduct;

        [ObservableProperty]
        private string priceText;

        partial void OnPriceTextChanged(string value)
        {
            currentProduct?.Price = decimal.TryParse(value, out decimal price) ? price : null;
        }

        [ObservableProperty]
        private string quantityText;

        partial void OnQuantityTextChanged(string value)
        {
            currentProduct?.Quantity = decimal.TryParse(value, out decimal quantity) ? quantity : null;
        }

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
        }


        [RelayCommand]
        private async Task CancelAsync()
        {
            HasError = false;
            ErrorMessages = new List<string>();
            CloseAsync();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            HasError = false;
            ErrorMessages = new List<string>();
            if (CurrentProduct != null)
            {
                if (PriceText.Length > 0
                    && !decimal.TryParse(PriceText, out decimal price))
                {
                    HasError |= true;
                    ErrorMessages.Add("Az ár mező érvénytelen számot tartalmaz!");
                }
                else
                    HasError |= false;

                if (QuantityText.Length > 0 
                    && !decimal.TryParse(QuantityText, out decimal quantity))
                {
                    HasError |= true;
                    ErrorMessages.Add("A készlet mező érvénytelen számot tartalmaz!");
                }
                else 
                    HasError |= false;

                if (HasError)
                    return;

                ProductInventoryTestApp.Services.ValidationResult validation = await _productService.SaveAsync(CurrentProduct);
                if (!validation.Success)
                {
                    HasError = true;
                    ErrorMessages = validation.ErrorMessages;
                }
                else
                {
                    HasError = false;
                    ErrorMessages = new List<string>();
                    CloseAsync();
                }
            }
        }

        private async Task CloseAsync()
        {
            await Shell.Current.GoToAsync("///ProductsPage");
        }
    }
}