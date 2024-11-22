using ARScrum.Model.AppModel;
using ARScrum.Model.AppModel.Authentication.Login;
using ARScrum.Model.AppModel.Authentication.SignUp;
using ARScrum.Services.Services.Authentication;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ARScrumWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserManagement _userManagement;

        public AuthenticationController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserManagement userManagement)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userManagement = userManagement;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUser? registerUser)
        {
            if (registerUser != null)
            {
                var token = await _userManagement.CreateUserWithTokenAsync(registerUser);
                if (token.IsSuccess)
                {

                    return StatusCode(StatusCodes.Status200OK,
                            new ARScrum.Model.AppModel.Response.Response { Status = "Success", Message = $"User created successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            new ARScrum.Model.AppModel.Response.Response { Status = "Error", Message = token?.Message?.ToString() });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ARScrum.Model.AppModel.Response.Response { Status = "Error", Message = "User cannot be null" });
            }

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel != null)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user is null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ARScrum.Model.AppModel.Response.Response { Status = "Error", Message = "Username or Password is incorrect" });
                var password = await _userManager.CheckPasswordAsync(user, loginModel.Password);
                if (user != null && password)
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles != null)
                    {
                        foreach (var role in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        var jwtToken = GetToken(authClaims);
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            expiration = jwtToken.ValidTo,
                            userId = user.Id,
                            userName = user.UserName,
                            email = user.Email,
                            Roles = userRoles,
                            user.FirstName,
                            user.LastName
                        });
                    }

                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ARScrum.Model.AppModel.Response.Response { Status = "Error", Message = "Username or Password is incorrect" });
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ARScrum.Model.AppModel.Response.Response { Status = "Error", Message = "User Name or Password cannot be null" });
            }

        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secrete"]));

            var token = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   expires: DateTime.Now.AddHours(1),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}
