using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecureProject.Shared;
using SecureProject.Shared.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { message = "User created successfully!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("isadmin", user.IsAdmin.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    return Ok(new { token = tokenHandler.WriteToken(token) });
                }
                catch (Exception ex)
                { }
            }
            return Unauthorized();
        }
        [HttpGet("userList")]
        public async Task<IActionResult> GetUserList()
        {
            var user = _userManager.Users.ToList();
            return Ok(user);
        }

        [HttpGet("grantadmin")]
        public async Task<IActionResult> grantadmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsAdmin = true;
            await _userManager.UpdateAsync(user);
            return Ok(true);
        }

        [HttpGet("revokeAdmin")]
        public async Task<IActionResult> revokeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsAdmin = false;
            await _userManager.UpdateAsync(user);
            return Ok(true);
        }
    }
}
