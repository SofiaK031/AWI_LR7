using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public interface IUserService
    {
        Task<ResponseModel<string>> Login(string username, string password);
        Task<ResponseModel<UserModel>> GetProfile(string username);
        Task<ResponseModel<bool>> UpdatePassword(string username, string oldPassword, string newPassword);
        Task<ResponseModel<bool>> DeleteProfile(string username);
    }
}