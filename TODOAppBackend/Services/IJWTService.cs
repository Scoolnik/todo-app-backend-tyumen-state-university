namespace TODOAppBackend.Services;

public interface IJWTService
{
	string CreateToken(int userId);
	bool TryGetUserId(string token, out int userId);
}
