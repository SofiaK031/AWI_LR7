using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public interface IFlowerStorageService
    {
        Task<ResponseModel<List<ProductModel>>> GetAllProducts();
        Task<ResponseModel<ProductModel>> GetProductByName(string name);
        Task<ResponseModel<ProductModel>> CreateProduct(ProductModel productItem);
        Task<ResponseModel<bool>> UpdateProductQuantity(string name, int quantity);
        Task<ResponseModel<bool>> DeleteProduct(string name);
    }
}