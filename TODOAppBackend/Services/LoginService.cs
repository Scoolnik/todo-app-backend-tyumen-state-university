﻿using TODOAppBackend.Models;

namespace TODOAppBackend.Services;

public interface ILoginService
{
	LoginResponse LoginIn(LoginRequest request);
}

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

			return LoginResponse.Success(token);
		}
		return LoginResponse.Fail();
	}
}
