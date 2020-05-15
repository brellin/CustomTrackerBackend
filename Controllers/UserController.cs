using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CustomTrackerBackend.Models;
using CustomTrackerBackend.Helpers;
using CustomTrackerBackend.Models.Inputs;
using System;

namespace CustomTrackerBackend.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class UserController : ControllerBase
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public UserController(UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User { UserName = input.Username };

                IdentityResult addUser = await userManager.CreateAsync(newUser, input.Password);
                if (addUser.Succeeded)
                {
                    User user = await userManager.FindByNameAsync(input.Username);
                    await signInManager.SignInAsync(user, false);
                    string token = TokenManager.GenerateToken(user);
                    return CreatedAtAction(nameof(Register), new { id = newUser.Id }, new { id = newUser.Id, token = token });
                }
                foreach (var error in addUser.Errors) ModelState.AddModelError(error.Code, error.Description);
                return ValidationProblem(ModelState);
            }
            return UnprocessableEntity(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult logInUser = await signInManager.PasswordSignInAsync(input.Username, input.Password, false, false);

                if (logInUser.Succeeded)
                {
                    User user = await userManager.FindByNameAsync(input.Username);
                    string token = TokenManager.GenerateToken(user);
                    return Ok(new { token = token });
                }
                return ValidationProblem($"Sign in with user \"{input.Username}\" failed");
            }
            return UnprocessableEntity(ModelState);
        }

    }
}