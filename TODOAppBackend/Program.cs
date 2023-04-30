using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TODOAppBackend;
using TODOAppBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen();

builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddAuthentication(x =>
{
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings")["Secret"])),
	};
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection()
	.UseAuthentication()
	.UseAuthorization();

//app.Map("/login/{username}", (string username) =>
//{
//	var claims = new List<Claim> { new Claim(ClaimTypes.UserData, username) };
//	// создаем JWT-токен
//	var jwt = new JwtSecurityToken(
//			claims: claims,
//			expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
//			signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

//	return new JwtSecurityTokenHandler().WriteToken(jwt);
//});

app.Map("/data", [Authorize] () => new { message = "Hello World!" });

app.MapControllers();

app.Run();

public class AuthOptions
{
	public const string ISSUER = "MyAuthServer"; // издатель токена
	public const string AUDIENCE = "MyAuthClient"; // потребитель токена
	const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
	public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
		new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}