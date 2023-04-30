using TODOAppBackend.Entities;

namespace TODOAppBackend.Services;

public interface IUserService
{
	IEnumerable<User> GetAll();
	User? GetById(int id);
	User? GetByLogin(string? login);
}
