using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public class FlowerStorageService : IFlowerStorageService
    {
        private List<ProductModel> _products;

        public FlowerStorageService()
        {
            _products = new List<ProductModel>
            {
                new ProductModel { Name = "Rose", Quantity = 50, Price = 150.00m },
                new ProductModel { Name = "Tulip", Quantity = 30, Price = 120.00m },
                new ProductModel { Name = "Orchid", Quantity = 20, Price = 250.00m },
                new ProductModel { Name = "Daisy", Quantity = 40, Price = 80.00m },
                new ProductModel { Name = "Sunflower", Quantity = 25, Price = 100.00m },
                new ProductModel { Name = "Lily", Quantity = 35, Price = 130.00m },
                new ProductModel { Name = "Peony", Quantity = 25, Price = 200.00m },
                new ProductModel { Name = "Lavender", Quantity = 60, Price = 90.00m },
                new ProductModel { Name = "Carnation", Quantity = 45, Price = 110.00m },
                new ProductModel { Name = "Jasmine", Quantity = 55, Price = 95.00m },
                new ProductModel { Name = "Soil for flowers", Quantity = 100, Price = 50.00m },
                new ProductModel { Name = "Flower pot", Quantity = 70, Price = 75.00m },
                new ProductModel { Name = "Watering can", Quantity = 20, Price = 150.00m },
                new ProductModel { Name = "Fertilizer mix", Quantity = 80, Price = 60.00m },
                new ProductModel { Name = "Plant spray bottle", Quantity = 50, Price = 85.00m }
            };
        }

        public async Task<ResponseModel<List<ProductModel>>> GetAllProducts()
        {
            var availableProducts = _products.Where(p => p.Quantity > 0).ToList();
            return await Task.FromResult(new ResponseModel<List<ProductModel>>(availableProducts, "List of all products in storage."));
        }

        public async Task<ResponseModel<ProductModel>> GetProductByName(string name)
        {
            var product = _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
                return await Task.FromResult(new ResponseModel<ProductModel>(product, "Product found."));
            else
                return await Task.FromResult(new ResponseModel<ProductModel>(null, "Product not found."));
        }

        public async Task<ResponseModel<ProductModel>> CreateProduct(ProductModel productItem)
        {
            var exists = _products.Any(p => p.Name.Equals(productItem.Name, StringComparison.OrdinalIgnoreCase));
            if (exists)
                return await Task.FromResult(new ResponseModel<ProductModel>(null, "Product already exists!"));

            _products.Add(productItem);
            return await Task.FromResult(new ResponseModel<ProductModel>(productItem, "Product created successfully."));
        }

        public async Task<ResponseModel<bool>> UpdateProductQuantity(string name, int quantity)
        {
            var product = _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                product.Quantity = quantity;
                return await Task.FromResult(new ResponseModel<bool>(true, "Product quantity updated."));
            }
            else
            {
                return await Task.FromResult(new ResponseModel<bool>(false, "Product not found."));
            }
        }
        
        public async Task<ResponseModel<bool>> DeleteProduct(string name)
        {
            var product = _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                _products.Remove(product);
                return await Task.FromResult(new ResponseModel<bool>(true, "Product deleted successfully."));
            }
            else
            {
                return await Task.FromResult(new ResponseModel<bool>(false, "Product not found."));
            }
        }
    }
}
