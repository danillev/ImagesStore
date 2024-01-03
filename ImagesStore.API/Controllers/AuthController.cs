using ImagesStore.API.Data;
using ImagesStore.API.Models;
using ImagesStore.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ImagesStore.API.Services;

namespace ImagesStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserGenericRepository _userRepository;
        private readonly AuthOptions _authOptions;

        public AuthController(ApplicationContext context)
        {
            _context = context;
            _userRepository = new UserGenericRepository(context);
            _authOptions = new AuthOptions();
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginData)
        {

            if (loginData == null)
            {
                return BadRequest("Invalid login or password");
            }

            User user = _userRepository.GetByEmail(loginData.Email).Result;
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            if (!IsPasswordValidate(loginData, user.PasswordHash))
            {
                return BadRequest(new { Message = loginData.PasswordHash });
            }

            var response = new
            {
                access_token = GeneratejwtToken(user.Email),
                email = user.Email
            };
            return Ok(response);
        }

        private bool IsPasswordValidate(User loginData, string passwordHash)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var IsPasswordValidate = hasher.VerifyHashedPassword(loginData, passwordHash, loginData.PasswordHash);
            return IsPasswordValidate == PasswordVerificationResult.Success;
        }

        private string GeneratejwtToken(string email)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, email) };
            var jwt = new JwtSecurityToken(issuer: _authOptions.ISSUER,
                audience: _authOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(3)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
