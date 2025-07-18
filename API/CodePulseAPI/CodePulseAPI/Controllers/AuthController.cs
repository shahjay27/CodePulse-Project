using CodePulseAPI.Models.DTO;
using CodePulseAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            //check email
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                //check password
                var checkPassResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPassResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    //create a token
                    var jwtToken = this.tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }

            ModelState.AddModelError("", "Email or Password is incorrect");

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create Identity User
            var identityUser = new IdentityUser
            {
                UserName = request?.email.Trim(),
                Email = request?.email.Trim()
            };

            var identityResult = await this.userManager.CreateAsync(identityUser, request?.password);

            if (identityResult.Succeeded)
            {
                //Add Role to user
                identityResult = await userManager.AddToRoleAsync(identityUser, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }
    }
}
