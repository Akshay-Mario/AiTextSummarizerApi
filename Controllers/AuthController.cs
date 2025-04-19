
using AiTextSummarizerApi.Data;
using AiTextSummarizerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiTextSummarizerApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthModel authModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == authModel.Username);

            if (existingUser != null)
                return Conflict("User a;ready exist");

            var user = new User
            {
                Username = authModel.Username,
                Password = authModel.Password,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registtered successfully", userId = user.id });
        }
    }
}