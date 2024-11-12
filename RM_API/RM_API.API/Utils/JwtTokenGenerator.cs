using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RM_API.Core.Entities;
using RM_API.Service.Tools;

namespace RM_API.API.Utils;

public class JwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly TimeZoneTool _timeZoneTool;

    public JwtTokenGenerator(IConfiguration configuration, TimeZoneTool timeZoneTool)
    {
        _configuration = configuration;
        _timeZoneTool = timeZoneTool;
    }

    public string GenerateToken(User user)
    {
        Guid userId = user.Id;
        string userName = user.UserName;
        string role = user.UserRole.RoleName.ToString();
        
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(ClaimTypes.Role, role) // Add the role claim here
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            jwtSettings["Issuer"],
            jwtSettings["Audience"],
            claims,
            expires: _timeZoneTool.ConvertUtcToAppTimeZone(DateTime.UtcNow).AddMinutes(Convert.ToDouble(jwtSettings["ExpirationInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}