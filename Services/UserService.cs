using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationLR7.Auth;
using WebApplicationLR7.Models;

namespace WebApplicationLR7.Services
{
    public class UserService : IUserService
    {
        List<UserModel> _users;
        IOptions<AuthSettings> _options;

        public async Task<ResponseModel<string>> Login(string username, string password)
        {
            try
            {
                var user = GetUserByUsername(username);

                if (user == null)
                {
                    throw new Exception($"{username} has not been found!");
                }
                else
                {
                    var passwordHashVerificationResult = new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.PasswordHash, password);

                    if (passwordHashVerificationResult == PasswordVerificationResult.Success)
                    {
                        // Generate JWT Token
                        var jwtToken = GenerateJwtToken(user);
                        var response = new ResponseModel<string>(jwtToken, $"JWT Token has been successfully generated!");
                        return response;
                    }
                    else
                    {
                        throw new Exception("Wrong password!");
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<string>(String.Empty, $"{ex.Message}");
                return await Task.FromResult(response);
            }
        }

        public string GenerateJwtToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Login", user.Login)
            };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(_options.Value.Expires),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey)), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private UserModel? GetUserByUsername(string username)
        {
            return _users.FirstOrDefault((u) => { return u.Login.Equals(username); }, null);
        }

        public async Task<ResponseModel<UserModel>> GetProfile(string username)
        {
            return await Task.FromResult(new ResponseModel<UserModel>(GetUserByUsername(username), "Profile"));
        }

        public async Task<ResponseModel<bool>> DeleteProfile(string username)
        {
            try
            {
                var user = _users.Find(u => u.Login.Equals(username));

                if (user != null)
                {
                    _users.Remove(user);

                    var response = new ResponseModel<bool>(true, $"Profile with username {username} has been deleted!");
                    return await Task.FromResult(response);
                }
                else
                {
                    throw new Exception("User with such login has not found!");
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<bool>(false, $"{ex.Message}");
                return await Task.FromResult(response);
            }
        }

        public async Task<ResponseModel<bool>> UpdatePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                var user = _users.Find(u => u.Login.Equals(username));
                var passwordHashVerificationResult = new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.PasswordHash, oldPassword);

                if (passwordHashVerificationResult == PasswordVerificationResult.Success)
                {
                    if (user != null)
                    {
                        _users.Remove(user);
                        user.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user, newPassword);
                        _users.Add(user);

                        var response = new ResponseModel<bool>(true, $"Profile with username {username} has been updated!");
                        return await Task.FromResult(response);
                    }
                    else
                    {
                        throw new Exception("User with such login has not found!");
                    }
                }
                else
                {
                    throw new Exception("You have entered wrong password!");
                }
            }
            catch (Exception ex)
            {
                var response = new ResponseModel<bool>(false, $"{ex.Message}");
                return await Task.FromResult(response);
            }
        }

        public UserService(IOptions<AuthSettings> options)
        {
            _options = options;
            _users = new List<UserModel>();
            var user1 = new UserModel();
            user1.Login = "admin";
            user1.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user1, "123123");

            var user2 = new UserModel();
            user2.Login = "emily";
            user2.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user2, "123123");

            var user3 = new UserModel();
            user3.Login = "michael";
            user3.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user3, "123123");

            var user4 = new UserModel();
            user4.Login = "sophia";
            user4.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user4, "123123");

            var user5 = new UserModel();
            user5.Login = "daniel";
            user5.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user5, "123123");

            var user6 = new UserModel();
            user6.Login = "olivia";
            user6.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user6, "123123");

            var user7 = new UserModel();
            user7.Login = "alexander";
            user7.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user7, "123123");

            var user8 = new UserModel();
            user8.Login = "isabella";
            user8.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user8, "123123");

            var user9 = new UserModel();
            user9.Login = "william";
            user9.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user9, "123123");

            var user10 = new UserModel();
            user10.Login = "mia";
            user10.PasswordHash = new PasswordHasher<UserModel>().HashPassword(user10, "123123");

            _users.Add(user1);
            _users.Add(user2);
            _users.Add(user3);
            _users.Add(user4);
            _users.Add(user5);
            _users.Add(user6);
            _users.Add(user7);
            _users.Add(user8);
            _users.Add(user9);
            _users.Add(user10);
        }
    }
}