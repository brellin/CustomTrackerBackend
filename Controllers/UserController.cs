using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomTrackerBackend.Helpers;
using CustomTrackerBackend.Models;
using CustomTrackerBackend.Models.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CustomTrackerBackend.Controllers
{
    [ApiController]
    [Route("/auth/[action]")]
    public class UserController : ControllerBase
    {
        private UserContext context;

        public UserController(UserContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                User user = await context.CreateUserAsync(input, input.Password);
                return Ok(new { id = user.Id });
            }
            return UnprocessableEntity(ModelState);
        }

        // [HttpPost]
        // public async Task<IActionResult> Login([FromBody] LoginInput input)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         Microsoft.AspNetCore.Identity.SignInResult logInUser = await signInManager.PasswordSignInAsync(input.Username, input.Password, false, false);

        //         if (logInUser.Succeeded)
        //         {
        //             User user = await userManager.FindByNameAsync(input.Username);
        //             string token = TokenManager.GenerateToken(user);
        //             return Ok(new { token = token });
        //         }
        //         return ValidationProblem($"Sign in with user \"{input.Username}\" failed");
        //     }
        //     return UnprocessableEntity(ModelState);
        // }

        // [Authorize]
        // [HttpGet]
        // public async Task<IActionResult> GetUser()
        // {
        //     User user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //     user.Issues = context.Issues.Where(i => i.UserId == user.Id).ToList();
        //     return Ok(user);
        // }

    }
}
