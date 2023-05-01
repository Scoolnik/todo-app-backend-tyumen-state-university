using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TODOAppBackend.Services;

public class JWTService : IJWTService
{
	private readonly AppSettings _appSettings;

	public JWTService(IOptions<AppSettings> appSettings)
	{
		_appSettings = appSettings.Value;
	}

	public string CreateToken(int userId)
	{
		var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
		var lifetime = _appSettings.TokenLifetime;

		var claims = new List<Claim>()
		{
			new ("userId", userId.ToString())
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.Add(lifetime),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		var jwt = tokenHandler.WriteToken(token);
		return jwt;
	}

	public bool TryGetUserId(string token, out int userId)
	{
		string jwt = token.StartsWith("Bearer") ? token.Substring(7) : token;
		var tokenHandler = new JwtSecurityTokenHandler();
		var userIdString = tokenHandler.ReadJwtToken(jwt).Claims.FirstOrDefault(x=>x.Type == "userId")?.Value;
		if (!string.IsNullOrWhiteSpace(userIdString))
		{
			userId = int.Parse(userIdString);
			return true;
		}
		userId = -1;
		return false;
	}
}
