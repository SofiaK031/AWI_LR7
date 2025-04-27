using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public class CartService : ICartService
    {
        private List<CartItemModel> _cartItems;
        private IFlowerDisplayService _flowerDisplayService;
        private IFlowerStorageService _flowerStorageService;

        public CartService(IFlowerDisplayService flowerDisplayService, IFlowerStorageService flowerStorageService)
        {
            _cartItems = new List<CartItemModel>();
            _flowerDisplayService = flowerDisplayService;
            _flowerStorageService = flowerStorageService;
        }

        public async Task<ResponseModel<List<CartItemModel>>> AddProduct(Guid userId, string productName, int quantity)
        {
            try
            {
                var displayProductResponse = await _flowerDisplayService.GetDisplayProductByName(productName);

                if (displayProductResponse.Data != null)
                {
                    // Товар є на вітрині
                    await _flowerDisplayService.SellProduct(productName, quantity);
                }
                else
                {
                    // Товару немає на вітрині - шукаємо на складі
                    var storageProductResponse = await _flowerStorageService.GetProductByName(productName);

                    if (storageProductResponse.Data == null || storageProductResponse.Data.Quantity < quantity)
                        throw new Exception("Product not found or not enough stock.");

                    // Оновлюємо кількість на складі
                    await _flowerStorageService.UpdateProductQuantity(productName, storageProductResponse.Data.Quantity - quantity);
                }

                // Додаємо товар у кошик
                var cartItem = _cartItems.FirstOrDefault(i => i.UserId == userId && i.Product.Name == productName);
                if (cartItem != null)
                {
                    cartItem.Product.Quantity += quantity;
                }
                else
                {
                    var productResponse = await _flowerStorageService.GetProductByName(productName);
                    _cartItems.Add(new CartItemModel
                    {
                        UserId = userId,
                        Product = new ProductModel
                        {
                            Name = productResponse.Data.Name,
                            Price = productResponse.Data.Price,
                            Quantity = quantity
                        }
                    });
                }

                var userCart = _cartItems.Where(i => i.UserId == userId).ToList();
                return new ResponseModel<List<CartItemModel>>(userCart, "Product added to cart.");
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<CartItemModel>>(null, $"Cart AddProduct Error: {ex.Message}");
            }
        }

        public async Task<ResponseModel<List<CartItemModel>>> GetProducts(Guid userId)
        {
            var items = _cartItems.Where(i => i.UserId == userId).ToList();
            return await Task.FromResult(new ResponseModel<List<CartItemModel>>(items, "Cart items retrieved."));
        }

        public async Task<ResponseModel<List<CartItemModel>>> UpdateProductUser(Guid oldUserId, Guid newUserId)
        {
            foreach (var item in _cartItems)
            {
                if (item.UserId == oldUserId)
                    item.UserId = newUserId;
            }
            return await Task.FromResult(new ResponseModel<List<CartItemModel>>(_cartItems, "User updated for cart items."));
        }

        public async Task<ResponseModel<List<CartItemModel>>> RemoveProduct(Guid userId, string productName)
        {
            var item = _cartItems.FirstOrDefault(i => i.UserId == userId && i.Product.Name == productName);
            if (item != null)
            {
                _cartItems.Remove(item);
                var items = _cartItems.Where(i => i.UserId == userId).ToList();
                return await Task.FromResult(new ResponseModel<List<CartItemModel>>(items, "Product removed from cart."));
            }
            else
            {
                return await Task.FromResult(new ResponseModel<List<CartItemModel>>(null, "Product not found in cart."));
            }
        }
    }
}
