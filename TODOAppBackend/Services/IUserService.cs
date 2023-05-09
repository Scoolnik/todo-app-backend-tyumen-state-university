using TODOAppBackend.Entities;

namespace TODOAppBackend.Services;

public interface IUserService
{
	IEnumerable<TM_User> GetAll();
	TM_User? GetById(int id);
	TM_User? GetByLogin(string? login);
}
