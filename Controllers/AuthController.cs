
using AiTextSummarizerApi.Data;
using AiTextSummarizerApi.DTOs;
using AiTextSummarizerApi.Models;
using AiTextSummarizerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiTextSummarizerApi.Controllers
{

    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthModel authModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == authModel.Username);

            if (existingUser != null)
                return Conflict("User already exist");

            var user = new User
            {
                Username = authModel.Username,
                Password = authModel.Password,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registtered successfully", userId = user.id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !user.IsActive || user.Password != request.Password)
            {
                return Unauthorized("Invalid credentials or account is inactive.");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenrateRefreshToken();

            var TokenModel = new UserToken
            {
                UserId = user.id,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            _context.UserTokens.Add(TokenModel);
            await _context.SaveChangesAsync();

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new LoginResponse
            {
                AccessToken = accessToken
            });

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshModel refreshModel)
        {

            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("refresh token missing");
            }

            var storedToken = await _context.UserTokens.FirstOrDefaultAsync(r => r.RefreshToken == refreshToken && r.ExpiryDate > DateTime.UtcNow);

            if (storedToken == null)
            {
                return Unauthorized("Invalid Token");
            }

            var user = await _context.Users.FindAsync(storedToken.UserId);

            if (user == null || !user.IsActive)
            {
                return Unauthorized("Inactive User");
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            return Ok(new LoginResponse { AccessToken = accessToken });

        }

    }
}