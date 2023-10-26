using Microsoft.AspNetCore.Mvc;
using CRUDWebAPI.Models.Authentication.SignUp;
using Microsoft.AspNetCore.Identity;

namespace CRUDWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null) 
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.Username
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                await _userManager.AddToRoleAsync(user, role);

                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
