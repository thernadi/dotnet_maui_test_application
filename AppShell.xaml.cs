using ProductInventoryTestApp.Pages;

namespace ProductInventoryTestApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
            nameof(ProductPage),
            typeof(ProductPage));
        }
    }
}
