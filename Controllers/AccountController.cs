using System.Security.Claims;
using System.Text;
using ahmed514essamAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _config;

    public AccountController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto login)
    {
        var adminEmail = _config["AdminUser:Email"];
        var adminPassword = _config["AdminUser:Password"];

        // لو هتستخدم تشفير زي BCrypt
        // if(!BCrypt.Net.BCrypt.Verify(login.Password, adminPassword)) return Unauthorized();

        if (login.Email != adminEmail || !BCrypt.Net.BCrypt.Verify(login.Password, adminPassword))
            return Unauthorized();

        // إنشاء JWT
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["jwt:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, login.Email),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["jwt:ExpirationMinutes"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["jwt:Issuer"],
            Audience = _config["jwt:Audience"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}