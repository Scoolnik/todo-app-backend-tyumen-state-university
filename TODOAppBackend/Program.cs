using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TODOAppBackend;
using TODOAppBackend.Entities;
using TODOAppBackend.Repository;
using TODOAppBackend.Services;

var policyName = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("authsettings.json");
var authSettings = builder.Configuration.GetSection("AuthSettings");
builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IRepository<TM_User>, BaseRepository<TM_User>>();
builder.Services.AddTransient<IRepository<TM_Task>, BaseRepository<TM_Task>>();

// Add services to the container.
builder.Services.Configure<AppAuthSettings>(authSettings);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen();

// Added CORS for 3000 port
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
                      builder =>
                      {
                          builder
                            .WithOrigins("http://localhost:3000") // specifying the allowed origin
                            .WithMethods("GET")
                            .WithMethods("DELETE")
                            .WithMethods("PUT")
                            .WithMethods("POST")// defining the allowed HTTP method
                            .AllowAnyHeader(); // allowing any header to be sent
                      });
});

builder.Services
	.AddScoped<IJWTService, JWTService>()
	.AddScoped<IUserService, UserService>()
	.AddScoped<ITaskService, TaskService>()
	.AddScoped<ILoginService, LoginService>()
	.AddScoped<ITaskMapperService, TaskMapperService>();


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
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings["Secret"])),
	};
});
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors(policyName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection()
	.UseAuthentication()
	.UseAuthorization();

app.MapControllers();

app.Run();