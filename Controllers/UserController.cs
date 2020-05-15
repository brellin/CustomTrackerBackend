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
        private RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<User> _userManager, SignInManager<User> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        private async Task<Tuple<IList<string>, User>> TokenizeUser(string username)
        {
            User user = await userManager.FindByNameAsync(username);
            IList<string> roles = await userManager.GetRolesAsync(user);
            return new Tuple<IList<string>, User>(roles, user);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterInput input)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User { UserName = input.Username };

                foreach (string role in input.Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        return BadRequest(new { error = $"Invalid role \"{role}\"" });
                };
                IdentityResult addUser = await userManager.CreateAsync(newUser, input.Password);
                if (addUser.Succeeded)
                {
                    IdentityResult addRoles = await userManager.AddToRolesAsync(newUser, input.Roles);
                    await signInManager.SignInAsync(newUser, false);
                    Tuple<IList<string>, User> userWithRoles = await TokenizeUser(input.Username);
                    string token = TokenManager.GenerateToken(userWithRoles);
                    return CreatedAtAction(nameof(Register), new { id = newUser.Id }, new { id = newUser.Id, token = token });
                }
                else
                {
                    foreach (var error in addUser.Errors) ModelState.AddModelError(error.Code, error.Description);
                    return ValidationProblem(ModelState);
                }
            }
            return UnprocessableEntity(ModelState);
        }

    }
}