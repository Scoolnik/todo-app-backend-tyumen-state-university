using TODOAppBackend.Models;

namespace TODOAppBackend.Services;

public class LoginService : ILoginService
{
	private readonly IUserService _userService;
	private readonly IJWTService _jwtService;

	public LoginService(IUserService userService, IJWTService jwtService)
	{
		_userService = userService;
		_jwtService = jwtService;
	}

	public LoginResponse LoginIn(LoginRequest request)
	{
		var dbUserInfo = _userService.GetByLogin(request.login);
		if (dbUserInfo != null && dbUserInfo.Password == request.password)
		{
			var token = _jwtService.CreateToken(dbUserInfo.Login);

			return LoginResponse.Success("Bearer " + token);
		}
		return LoginResponse.Fail();
	}
}

