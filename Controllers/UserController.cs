using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationLR7.Models;
using WebApplicationLR7.Services;

namespace WebApplicationLR7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<ResponseModel<string>> Login(string login, string password)
        {
            return await _userService.Login(login, password);
        }

        [HttpGet("GetProfile")]
        [Authorize]
        public async Task<ResponseModel<UserModel>> GetProfile()
        {
            var userLogin = User.FindFirst("Login").Value;
            var result = await _userService.GetProfile(userLogin);
            result.Data.PasswordHash = "hidden";
            return result;
        }

        [HttpDelete("DeleteProfile")]
        [Authorize]
        public async Task<ResponseModel<bool>> DeleteProfile(string username)
        {
            var userLogin = User.FindFirst("Login").Value;
            if (username.Equals(userLogin))
            {
                return await _userService.DeleteProfile(username);
            }
            else
            {
                return await Task.FromResult(new ResponseModel<bool>(false, $"You do not have permission to delete such a profile!"));
            }
        }

        [HttpPut("UpdatePassword")]
        [Authorize]
        public async Task<ResponseModel<bool>> UpdatePassword(string username, string oldPassword, string newPassword)
        {
            var userLogin = User.FindFirst("Login").Value;
            if (username.Equals(userLogin))
            {
                return await _userService.UpdatePassword(username, oldPassword, newPassword);
            }
            else
            {
                return await Task.FromResult(new ResponseModel<bool>(false, $"You do not have permission to update such a profile!"));
            }
        }
    }
}