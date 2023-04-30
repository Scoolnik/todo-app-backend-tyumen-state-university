using TODOAppBackend.Models;

namespace TODOAppBackend.Services;

public interface ILoginService
{
	LoginResponse LoginIn(LoginRequest request);
}

