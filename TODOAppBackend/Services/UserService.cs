using TODOAppBackend.Entities;

namespace TODOAppBackend.Services;

public interface IUserService
{
	IEnumerable<User> GetAll();
	User? GetById(int id);
	User? GetByLogin(string? login);
}

public class UserService : IUserService
{
	private List<User> _users = new()
	{
		new User { Id = 1, Login = "string", Password = "string" }
	};

	public IEnumerable<User> GetAll() => _users;

	public User? GetById(int id) => _users.FirstOrDefault(x => x.Id == id);

	public User? GetByLogin(string? login) => _users.FirstOrDefault(x => x.Login == login);
}
