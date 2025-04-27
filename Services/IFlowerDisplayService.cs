using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public interface IFlowerDisplayService
    {
        Task<ResponseModel<List<ProductModel>>> GetDisplayProducts();
        Task<ResponseModel<ProductModel>> GetDisplayProductByName(string name);
        Task<ResponseModel<bool>> SellProduct(string name, int quantity);
        Task<ResponseModel<bool>> UpdateProductQuantity(string name, int quantity);
        Task<ResponseModel<bool>> DeleteProduct(string name);
    }
}