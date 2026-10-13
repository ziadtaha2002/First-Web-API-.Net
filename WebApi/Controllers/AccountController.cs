using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MvcMovie.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        // Iconfiguration is used to access configuration settings,
        // such as JWT settings, from appsettings.json or other configuration sources.
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<ApplicationUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO userFromRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = new ApplicationUser();
            user.UserName = userFromRegister.UserName;
            user.Email = userFromRegister.Email;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userFromRegister.Password);
            IdentityResult result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                return Ok("Creation successful");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
        }


        private string CreateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>
         {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
         };

            string key = _configuration["Jwt:Key"]!;
            string issuer = _configuration["Jwt:Issuer"]!;
            string audience = _configuration["Jwt:Audience"]!;

            SymmetricSecurityKey securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            SigningCredentials credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>("Jwt:DurationInMinutes"));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            //check
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await _userManager.FindByNameAsync(loginDto.UserName);
              if (user == null)
                {
                  return Unauthorized(new General_Response<Object>
                   {
                     Data = null,
                     isSuccess = false,
                     Message = "Invalid username or password"
                   });
                }
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new General_Response<Object>
                {
                    Data = null,
                    isSuccess = false,
                    Message = "Invalid username or password"
                });
                
            }

            string token = CreateToken(user);
            LoginResponseDTO loginResponseDTO    = new LoginResponseDTO
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<int>("Jwt:DurationInMinutes"))
            };

            return Ok(new General_Response<LoginResponseDTO>
            {
                Data = loginResponseDTO,
                isSuccess = true,
                Message = "Login successful"
            });
        }
    }

}
