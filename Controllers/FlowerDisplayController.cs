using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationLR7.Models;
using WebApplicationLR7.Services;

namespace WebApplicationLR7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlowerDisplayController : ControllerBase
    {
        private readonly IFlowerDisplayService _flowerDisplayService;

        public FlowerDisplayController(IFlowerDisplayService flowerDisplayService)
        {
            _flowerDisplayService = flowerDisplayService;
        }

        [HttpGet("GetAllDisplay")]
        [Authorize]
        public async Task<ResponseModel<List<ProductModel>>> GetAllDisplay()
        {
            return await _flowerDisplayService.GetDisplayProducts();
        }

        [HttpGet("GetProductDisplay")]
        [Authorize]
        public async Task<ResponseModel<ProductModel>> GetProductDisplay(string name)
        {
            return await _flowerDisplayService.GetDisplayProductByName(name);
        }

        [HttpPost("SellProduct")]
        [Authorize]
        public async Task<ResponseModel<bool>> SellProduct(string name, int quantity)
        {
            return await _flowerDisplayService.SellProduct(name, quantity);
        }

        [HttpPut("UpdateQuantityDisplay")]
        [Authorize]
        public async Task<ResponseModel<bool>> UpdateQuantityDisplay(string name, int quantity)
        {
            return await _flowerDisplayService.UpdateProductQuantity(name, quantity);
        }

        [HttpDelete("DeleteProductDisplay")]
        [Authorize]
        public async Task<ResponseModel<bool>> DeleteProductDisplay(string name)
        {
            return await _flowerDisplayService.DeleteProduct(name);
        }
    }
}
