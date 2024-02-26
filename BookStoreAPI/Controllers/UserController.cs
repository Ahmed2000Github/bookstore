using BookStoreAPI.Dto.UserDto;
using BookStoreAPI.Services;
using Core.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        const string UserRole = "USER";
        const string AdminRole = "ADMIN";
        private readonly IConfiguration configuration;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMemoryCache memoryCache;

        public UserController(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMemoryCache memoryCache
            )
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.memoryCache = memoryCache;
        }
        [HttpPost]
        public async Task<ActionResult> CreateAdmin()
        {
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                var newRole = new IdentityRole(AdminRole);
                await roleManager.CreateAsync(newRole);
            }
            var user = new AppUser
            {
                Email = "Admin@gmail.com",
                UserName = "Admin@1234",
                AvatarUrl = "/Avatars/default.png",
            };
            var result = userManager.CreateAsync(user, "12345678");

            if (result.IsCompletedSuccessfully)
            {
                await userManager.AddToRoleAsync(user, AdminRole);
                var authData = CreateToken(user, AdminRole);
                return Ok(authData);
            }
            else
            {
                await Console.Out.WriteLineAsync(result.Result.Errors.ToString());
                return Ok("Admin not created.");
            }
            
        }
        [HttpGet, Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var usersList = userManager.Users.ToList();
            var usersDtoList = new List<UserResponseDto>();
            foreach (var user in usersList)
            {
                var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                if (role == UserRole)
                {
                    usersDtoList.Add(new UserResponseDto()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        ImageUrl = user.AvatarUrl,
                        Email = user.Email,
                    });
                }

            }
            usersList.Select(user => new UserResponseDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                ImageUrl = user.AvatarUrl,
                Email = user.Email,
            });
            return Ok(usersDtoList);
        }

        [HttpPost]
        public async Task<ActionResult> LoginAdmin(AdminLoginParamsDto parameters)
        {
            var user = await userManager.FindByNameAsync(parameters.Login);
            if (user is null)
            {
                user = await userManager.FindByEmailAsync(parameters.Login);
                if (user is null)
                {
                    return NotFound("User not found.");
                }
            }
            if (await userManager.CheckPasswordAsync(user, parameters.Password))
            {
                var authData = CreateToken(user, AdminRole);
                return Ok(authData);
            }
            else
            {
                return BadRequest("The password is incorrect.");
            }

        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLoginParamsDto parameters)
        {
            var user = await userManager.FindByNameAsync(parameters.Login);
            if (user is null)
            {
                user = await userManager.FindByEmailAsync(parameters.Login);
                if (user is null)
                {
                    return NotFound("User not found.");
                }
            }
            if (await userManager.CheckPasswordAsync(user, parameters.Password))
            {
                var authData = CreateToken(user, UserRole);
                return Ok(authData);
            }
            return BadRequest("The password is incorrect.");
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserRegistrationParamsDto parameters)
        {

            if (!await roleManager.RoleExistsAsync(UserRole))
            {
                var newRole = new IdentityRole(UserRole);
                await roleManager.CreateAsync(newRole);
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            };
            memoryCache.Set(parameters.Email, parameters, cacheEntryOptions);
            sendEmail(parameters.Email);
            return Ok("Email sent successfully.");
        }

        [HttpPost]
        public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailParamsDto parameters)
        {
            var _result = VerifyKey(parameters.Email, parameters.Key);
            if (_result)
            {
                if (memoryCache.TryGetValue(parameters.Email, out UserRegistrationParamsDto _parameters))
                {
                    memoryCache.Remove(parameters.Email);
                    var user = new AppUser
                    {
                        Email = _parameters.Email,
                        UserName = _parameters.UserName,
                        AvatarUrl = "/Avatars/default.png",
                    };
                    var result = await userManager.CreateAsync(user, _parameters.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, UserRole);
                        var authData = CreateToken(user, UserRole);
                        return Ok(authData);
                    }
                }
            }
            return BadRequest("Key or Email not valid.");
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordParamsDto parameters)
        {
            var user = await userManager.FindByEmailAsync(parameters.Email);
            if (user is null)
            {
                return NotFound("User with this email not found.");
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            };
            memoryCache.Set(parameters.Email, parameters.NewPassword, cacheEntryOptions);
            sendEmail(parameters.Email);
            return Ok("Email verified.");
        }

        [HttpPost]
        public async Task<ActionResult> VerifyEmailForPassword([FromBody] VerifyEmailParamsDto parameters)
        {
            var result = VerifyKey(parameters.Email, parameters.Key);
            if (result)
            {
                if (memoryCache.TryGetValue(parameters.Email, out string newPassword))
                {
                    var user = await userManager.FindByEmailAsync(parameters.Email);
                    if (user is null)
                    {
                        return NotFound("User with this email not found.");
                    }
                    string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordResult = await userManager.ResetPasswordAsync(user, resetToken, newPassword);
                    if (resetPasswordResult.Succeeded)
                    {
                        var authData = CreateToken(user, UserRole);
                        return Ok(authData);
                    }
                }
            }
            return BadRequest("Key or Email not valid");
        }

        [HttpDelete("{id}"), Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete user.");
            }

        }

        private AuthenticationResponseDto CreateToken(AppUser user, string role)
        {
            List<Claim> accessTokenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.UserData,user.Id),
                new Claim(ClaimTypes.Uri,user.AvatarUrl),
                new Claim(ClaimTypes.Role, role)
            };
            List<Claim> refreshTokenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var accessToken = new JwtSecurityToken(
                claims: accessTokenClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var refreshToken = new JwtSecurityToken(
                claims: refreshTokenClaims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds);
            var v1 = new JwtSecurityTokenHandler().WriteToken(accessToken);
            var v2 = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            var auth = new AuthenticationResponseDto
            {
                AccessToken = v1,
                RefreshToken = v2,
                AccessTokenExpireIn = DateTime.Now.AddDays(1),
                RefreshTokenExpireIn = DateTime.Now.AddDays(2),
            };
            return auth;
        }
        private void sendEmail(string email)
        {
            var verificationKey = GenerateRandomDigits(6);
            //Console.WriteLine("#############"+verificationKey);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(20)
            };
            var result = EmailSender.Default.send(email, "BookStore email verification",
                "Use this key < " + verificationKey + " > to verify your email");
            if (result)
            {
                memoryCache.Set(email + "key", verificationKey, cacheEntryOptions);
            }

        }
        private bool VerifyKey(string email, string key)
        {
            if (memoryCache.TryGetValue(email + "key", out string verificationKey))
            {
                if (key == verificationKey)
                {
                    memoryCache.Remove(email + "key");
                    return true;
                }
            }
            return false;
        }
        private string GenerateRandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

    }
}
