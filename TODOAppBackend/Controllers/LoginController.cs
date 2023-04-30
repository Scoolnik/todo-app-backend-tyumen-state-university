using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODOAppBackend.Models;
using TODOAppBackend.Services;

namespace TODOAppBackend.Controllers;

[ApiController]
[Route("api/auth/login")]
public class LoginController : ControllerBase
{
	private readonly ILoginService _loginService;

	public LoginController(ILoginService loginService)
	{
		_loginService = loginService;
	}

	[HttpPost]
	public IActionResult Login([FromBody] LoginRequest request)
	{
		return Ok(_loginService.LoginIn(request));
	}
}

