namespace TODOAppBackend.Services;

public interface IJWTService
{
	string CreateToken(string userId);
}
