using DemoAuthAPI.Models;
using DemoAuthAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DemoAuthContext _context;

        public AuthenticationController(IConfiguration config, DemoAuthContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserInfo _userData)
        {
            if (_userData != null && _userData.UserName != null && _userData.Password != null)
            {
                var user = await GetUser(_userData.UserName, _userData.Password);

                if (user != null)
                {
                    string accessToken = AuthenticationHelper.GenerateJwtToken(_configuration, user);
                    string refreshToken = AuthenticationHelper.GenerateRefreshToken();

                    var userToken = new UserToken()
                    {
                        UserId = user.UserId,
                        RefreshToken = refreshToken,
                        ExpiredAt = DateTime.UtcNow.AddMonths(1)
                    };
                    await _context.UserTokens.AddAsync(userToken);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken
                    });
                }
                else
                {
                    return BadRequest("Unauthorized");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            // Validate the refresh token
            var refreshToken = _context.UserTokens.Where(ut => ut.RefreshToken.Equals(request.RefreshToken)).First();

            if (refreshToken == null || refreshToken.ExpiredAt < DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            // Generate a new access token (JWT)
            var user = _context.UserInfos.Where(ui => ui.UserId == refreshToken.UserId).First();
            var newJwtToken = AuthenticationHelper.GenerateJwtToken(_configuration, user);

            return Ok(new
            {
                Token = newJwtToken,
                RefreshToken = refreshToken.RefreshToken,
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout(TokenRequest request)
        {
            // Remove the refresh token from the database
            var refreshToken = _context.UserTokens.SingleOrDefault(rt => rt.RefreshToken == request.RefreshToken);

            if (refreshToken != null)
            {
                _context.UserTokens.Remove(refreshToken);
                _context.SaveChanges();
            }

            return Ok(new { message = "Logout successful" });
        }

        public class LogoutModel
        {
            public string RefreshToken { get; set; }
        }


        public class TokenRequest
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }


        private async Task<UserInfo> GetUser(string username, string password)
        {
            return await _context.UserInfos.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
