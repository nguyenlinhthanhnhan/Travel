using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Travel.Application.Common.Interfaces;
using Travel.Application.Dtos.User;
using Travel.Domain.Entities;
using Travel.Identity.Helpers;

namespace Travel.Identity.Services;

public class UserService:IUserService
{
    private readonly IApplicationDbContext _context;
    private readonly AuthSettings _authSettings;

    public UserService(IOptions<AuthSettings> appSettings) => _authSettings = appSettings.Value;

    public AuthenticateResponse? Authenticate(AuthenticateRequest model)
    {
        // This use for example, when use in real project, password have to encrypted, cannot compare like as this
        var user = _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

        if (user == null) return null;

        var token = GenerateJwtToken(user);

        return new AuthenticateResponse(user, token);
    }

    public User? GetById(int id) => _context.Users.FirstOrDefault(x => x.Id == id);

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}