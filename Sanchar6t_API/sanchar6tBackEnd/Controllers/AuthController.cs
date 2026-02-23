using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sanchar6tBackEnd.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using sanchar6tBackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using sanchar6tBackEnd.Data;
using Microsoft.EntityFrameworkCore;
namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ILogin _authService;
        private readonly IConfiguration _configuration;
        private readonly Sanchar6tDbContext _context;

        public AuthController(ILogin authService, IConfiguration configuration,Sanchar6tDbContext context)
        {
            _authService = authService;
            _configuration = configuration;
            this._context = context;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var user = login.MobileNo != ""
                    ? await _authService.AuthenticateUser(login.Email,login.MobileNo, login.Password)
                    : await _authService.AuthenticateUser(login.Email, login.MobileNo, login.Password);

                if (user == null || user.Data is not List<sanchar6tBackEnd.Models.VwUser> userData || userData.Count == 0)
                    return Unauthorized(new { message = "Invalid credentials" });
                var firstUser = userData.FirstOrDefault();
                if (firstUser == null)
                    return Unauthorized(new { message = "Access denied: Only admin can log in." });
                var token = GenerateJwtToken(user);
                var walletvalues = _context.VwWallets.Where(m => m.UserId == firstUser.UserId).ToListAsync();
                return Ok(new { Token = token, Message = "Admin login successful",
                userData = firstUser,wallet = walletvalues});
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = result.Message });
            }
        }

        private string GenerateJwtToken(CommonRsult user)
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                Console.WriteLine("JWT Key is missing from configuration!");
                return string.Empty;
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (user.Data is not List<sanchar6tBackEnd.Models.VwUser> userData || userData.Count == 0)
            {
                Console.WriteLine("No valid user data found in CommonRsult!");
                return string.Empty;
            }
            var firstUser = userData.First();
            if (firstUser == null)
            {
                Console.WriteLine("First user is null!");
                return string.Empty;
            }
            Console.WriteLine($"Generating token for admin user: {firstUser.FirstName}, ID: {firstUser.UserId}");
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, firstUser.FirstName ?? ""),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim("UserId", firstUser.UserId.ToString()),
                 new Claim("UserType", firstUser.Usertype.ToString())
             };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($"Generated Token: {tokenString}");
            return tokenString;
        }
    }
}