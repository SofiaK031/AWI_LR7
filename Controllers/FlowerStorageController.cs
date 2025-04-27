using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationLR7.Models;
using WebApplicationLR7.Services;

namespace WebApplicationLR7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlowerStorageController : ControllerBase
    {
        private readonly IFlowerStorageService _flowerStorageService;

        public FlowerStorageController(IFlowerStorageService flowerStorageService)
        {
            _flowerStorageService = flowerStorageService;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ResponseModel<List<ProductModel>>> GetAll()
        {
            return await _flowerStorageService.GetAllProducts();
        }

        [HttpGet("GetProduct")]
        [Authorize]
        public async Task<ResponseModel<ProductModel>> GetProduct(string name)
        {
            return await _flowerStorageService.GetProductByName(name);
        }

        [HttpPost("CreateProduct")]
        [Authorize]
        public async Task<ResponseModel<ProductModel>> CreateProduct([FromBody] ProductModel product)
        {
            return await _flowerStorageService.CreateProduct(product);
        }

        [HttpPut("UpdateQuantity")]
        [Authorize]
        public async Task<ResponseModel<bool>> UpdateQuantity(string name, int quantity)
        {
            return await _flowerStorageService.UpdateProductQuantity(name, quantity);
        }

        [HttpDelete("DeleteProduct")]
        [Authorize]
        public async Task<ResponseModel<bool>> DeleteProduct(string name)
        {
            return await _flowerStorageService.DeleteProduct(name);
        }
    }
}
