using ProductInventoryTestApp.ViewModels;

namespace ProductInventoryTestApp.Pages
{
    public partial class ProductsPage : ContentPage
    {
        private readonly ProductsViewModel _viewModel;

        public ProductsPage(ProductsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadProductsAsync();

            ProductsCollectionView.ItemsSource = _viewModel.Products;
        }
    }
}