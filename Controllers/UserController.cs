
using System.Security.Claims;
using AiTextSummarizerApi.Data;
using AiTextSummarizerApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTextSummarizerApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {


        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            {
                return Unauthorized(new
                {
                    Message = "User information could not be retrieved from token."
                });
            }
            
            return Ok(new UserInfo
            {
                UserId = userId,
                Email = email
            });
        }
    }
}