using ProductInventoryTestApp.Entities;
using ProductInventoryTestApp.ViewModels;

namespace ProductInventoryTestApp.Pages
{
    public partial class ProductPage : ContentPage, IQueryAttributable
    {
        private readonly ProductViewModel _viewModel;

        public ProductPage(ProductViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedProduct", out object product))
            {
                _viewModel.CurrentProduct = (Product)product;
                _viewModel.PriceText = _viewModel.CurrentProduct.Price.ToString();
                _viewModel.QuantityText = _viewModel.CurrentProduct.Quantity.ToString();
            }
        }
    }
}