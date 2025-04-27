using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationLR7.Models;
using WebApplicationLR7.Services;

namespace WebApplicationLR7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetProducts")]
        [Authorize]
        public async Task<ResponseModel<List<CartItemModel>>> GetProducts()
        {
            var userIdStr = User.FindFirst("Id").Value;

            if (userIdStr != null)
            {
                var userGuid = new Guid(userIdStr);
                return await _cartService.GetProducts(userGuid);
            }
            else
            {
                return new ResponseModel<List<CartItemModel>>(null, "Can't parse ID from JWT token.");
            }
        }

        [HttpPost("AddProduct")]
        [Authorize]
        public async Task<ResponseModel<List<CartItemModel>>> AddProduct(string productName, int quantity)
        {
            var userIdStr = User.FindFirst("Id").Value;

            if (userIdStr != null)
            {
                var userGuid = new Guid(userIdStr);
                return await _cartService.AddProduct(userGuid, productName, quantity);
            }
            else
            {
                return new ResponseModel<List<CartItemModel>>(null, "Can't parse ID from JWT token.");
            }
        }

        [HttpDelete("RemoveProduct")]
        [Authorize]
        public async Task<ResponseModel<List<CartItemModel>>> RemoveProduct(string productName)
        {
            var userIdStr = User.FindFirst("Id").Value;

            if (userIdStr != null)
            {
                var userGuid = new Guid(userIdStr);
                return await _cartService.RemoveProduct(userGuid, productName);
            }
            else
            {
                return new ResponseModel<List<CartItemModel>>(null, "Can't parse ID from JWT token.");
            }
        }

        [HttpPut("UpdateProductUser")]
        [Authorize]
        public async Task<ResponseModel<List<CartItemModel>>> UpdateProductUser(Guid oldUser, Guid newUser)
        {
            var userLogin = User.FindFirst("Login").Value;

            if (userLogin == "admin")
            {
                return await _cartService.UpdateProductUser(oldUser, newUser);
            }
            else
            {
                return new ResponseModel<List<CartItemModel>>(null, "You don't have enough permissions!");
            }
        }
    }
}