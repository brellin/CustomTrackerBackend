using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomTracker.Helpers;
using CustomTracker.Models;
using CustomTracker.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomTracker.Controllers
{
    [ApiController]
    [Route("/auth/[action]")]
    public class UserController : ControllerBase
    {
        private readonly UserContext context;

        public UserController(UserContext _context) { context = _context; }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await context.CreateUserAsync(input);
                    string token = user.GenerateToken();
                    return Created(nameof(Register), new { id = user.Id, token = token });
                }
                catch (Exception exception) { return BadRequest(exception); }
            }
            return UnprocessableEntity(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            try
            {
                User user = await context.SignInUser(input);
                string token = user.GenerateToken();
                return Ok(new { token = token });
            }
            catch (Exception exception) { return ValidationProblem(exception.Message); }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Me()
        {
            try
            {
                // Get user's id
                User user = await context.Users.FirstAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                // Get user matching id
                user.Issues = context.Issues.Where(i => i.UserId == user.Id).ToList();
                // Return matching user
                return Ok(user);
            }
            catch (Exception exception) { return ValidationProblem(exception.Message); }
        }
    }
}
