using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomTrackerBackend.Helpers;
using CustomTrackerBackend.Models;
using CustomTrackerBackend.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomTrackerBackend.Controllers
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
                    return Created(nameof(Register), new { token = token, user = user });
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
                return Ok(new { token = token, user = user });
            }
            catch (Exception exception) { return ValidationProblem(exception.Message); }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Me()
        {
            try
            {
                // Get user matching user's id
                User user = await context.Users.FirstAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
                // populate Issues matching user's id
                user.Issues = context.Issues.Where(i => i.UserId == user.Id).ToList();
                // populate Groups matching user's id
                user.Groups = context.Groups.Where(g => g.OwnerId == user.Id).ToList();
                // Return matching user
                return Ok(user);
            }
            catch (Exception exception) { return ValidationProblem(exception.Message); }
        }
    }
}
