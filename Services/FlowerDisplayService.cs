using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public class FlowerDisplayService : IFlowerDisplayService
    {
        private List<ProductModel> _displayProducts;
        private IFlowerStorageService _flowerStorageService;

        public FlowerDisplayService(IFlowerStorageService flowerStorageService)
        {
            _flowerStorageService = flowerStorageService;

            _displayProducts = new List<ProductModel>
            {
                new ProductModel { Name = "Rose", Quantity = 5, Price = 150.00m },
                new ProductModel { Name = "Tulip", Quantity = 8, Price = 120.00m },
                new ProductModel { Name = "Daisy", Quantity = 7, Price = 80.00m },
                new ProductModel { Name = "Sunflower", Quantity = 6, Price = 100.00m },
                new ProductModel { Name = "Lily", Quantity = 8, Price = 130.00m },
                new ProductModel { Name = "Flower pot", Quantity = 10, Price = 75.00m },
                new ProductModel { Name = "Fertilizer mix", Quantity = 15, Price = 60.00m },
                new ProductModel { Name = "Hydrangea", Quantity = 6, Price = 180.00m },
                new ProductModel { Name = "Begonia", Quantity = 8, Price = 85.00m },
                new ProductModel { Name = "Chrysanthemum", Quantity = 7, Price = 140.00m }
            };
        }

        public async Task<ResponseModel<List<ProductModel>>> GetDisplayProducts()
        {
            var availableProducts = _displayProducts.Where(p => p.Quantity > 0).ToList();
            return await Task.FromResult(new ResponseModel<List<ProductModel>>(availableProducts, "Products available in the display."));
        }

        public async Task<ResponseModel<ProductModel>> GetDisplayProductByName(string name)
        {
            var product = _displayProducts.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
                return await Task.FromResult(new ResponseModel<ProductModel>(product, "Product found in display."));
            else
                return await Task.FromResult(new ResponseModel<ProductModel>(null, "Product not found in display."));
        }

        public async Task<ResponseModel<bool>> SellProduct(string name, int quantity)
        {
            try
            {
                var displayProduct = _displayProducts.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                var storageProductResponse = await _flowerStorageService.GetProductByName(name);

                if (storageProductResponse.Data == null)
                    throw new Exception("Product not found in storage.");

                var storageProduct = storageProductResponse.Data;

                if (displayProduct != null)
                {
                    if (displayProduct.Quantity >= quantity)
                    {
                        // Вистачає на вітрині
                        displayProduct.Quantity -= quantity;
                    }
                    else
                    {
                        // Не вистачає на вітрині, беремо залишок зі складу
                        int remainingFromStorage = quantity - displayProduct.Quantity;

                        if (storageProduct.Quantity >= remainingFromStorage)
                        {
                            displayProduct.Quantity = 0; // Зняли все з вітрини
                            storageProduct.Quantity -= remainingFromStorage;
                            await _flowerStorageService.UpdateProductQuantity(name, storageProduct.Quantity);
                        }
                        else
                        {
                            throw new Exception("Not enough stock in total (display + storage).");
                        }
                    }
                }
                else
                {
                    // Немає товару на вітрині - беремо зі складу
                    if (storageProduct.Quantity >= quantity)
                    {
                        storageProduct.Quantity -= quantity;
                        await _flowerStorageService.UpdateProductQuantity(name, storageProduct.Quantity);
                    }
                    else
                    {
                        throw new Exception("Not enough stock in storage.");
                    }
                }

                return await Task.FromResult(new ResponseModel<bool>(true, "Sale completed successfully."));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel<bool>(false, $"Sale error: {ex.Message}"));
            }
        }

        public async Task<ResponseModel<bool>> UpdateProductQuantity(string name, int quantity)
        {
            var product = _displayProducts.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                product.Quantity = quantity;
                return await Task.FromResult(new ResponseModel<bool>(true, "Display product quantity updated."));
            }
            else
            {
                return await Task.FromResult(new ResponseModel<bool>(false, "Product not found in display."));
            }
        }

        public async Task<ResponseModel<bool>> DeleteProduct(string name)
        {
            var product = _displayProducts.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                _displayProducts.Remove(product);
                return await Task.FromResult(new ResponseModel<bool>(true, "Display product deleted successfully."));
            }
            else
            {
                return await Task.FromResult(new ResponseModel<bool>(false, "Product not found in display."));
            }
        }
    }
}
