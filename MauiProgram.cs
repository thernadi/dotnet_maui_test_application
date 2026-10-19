using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductInventoryTestApp.Entities;
using ProductInventoryTestApp.Repositories;
using ProductInventoryTestApp.Services;


namespace ProductInventoryTestApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string configFile = "appsettings.json";

#if DEBUG
            builder.Logging.AddDebug();
            configFile = "appsettings.Development.json";
#endif
            using Stream configStream = FileSystem.OpenAppPackageFileAsync(configFile).Result;
            var config = new ConfigurationBuilder()
                .AddJsonStream(configStream)
                .Build();
            builder.Configuration.AddConfiguration(config);

            string? connectionString =
            builder.Configuration.GetConnectionString("inventoryDb");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'inventoryDb' not found.");
            }

            //SQLite connection string
            if (connectionString.StartsWith("Filename=@Path;", StringComparison.OrdinalIgnoreCase))
            {
                string[] parts = connectionString.Split(";", 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    connectionString = $"Filename={Path.Combine(FileSystem.AppDataDirectory, parts[1])}";
                }
                else
                {
                    throw new InvalidOperationException(
                        "Connection string 'inventoryDb' is not valid.");
                }
            }

            builder.Services.AddDbContext<InventoryData.InventoryDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<ViewModels.ProductsViewModel>();
            builder.Services.AddScoped<Pages.ProductsPage>();

            builder.Services.AddTransient<ViewModels.ProductViewModel>();
            builder.Services.AddTransient<Pages.ProductPage>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                //TESZT ADATOK
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                Console.WriteLine($"Affected records: {productService.GenerateTestDataAsync().GetAwaiter().GetResult()}");
                /*
                List<Product> products = await productService.GetProductsAsync(null, null, 1, 50)
                .GetAwaiter()
                .GetResult();
                */
            }

            return app;
        }
    }
}


/*
TODO:
6. Leadás

A megoldást publikus Git repository formájában kérjük leadni.

A repository tartalmazzon egy rövid README.md fájlt, amelyben szerepel:

· milyen technikai döntéseket hoztál

· volt-e olyan megoldás vagy részfeladat, amelyet több idő rendelkezésre állása esetén másképp valósítanál meg

· milyen további fejlesztéseket tennél bele


 */