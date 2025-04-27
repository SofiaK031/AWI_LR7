using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public interface ICartService
    {
        Task<ResponseModel<List<CartItemModel>>> GetProducts(Guid userId);
        Task<ResponseModel<List<CartItemModel>>> AddProduct(Guid userId, string productName, int quantity);
        Task<ResponseModel<List<CartItemModel>>> RemoveProduct(Guid userId, string productName);
        Task<ResponseModel<List<CartItemModel>>> UpdateProductUser(Guid oldUserId, Guid newUserId);
    }
}