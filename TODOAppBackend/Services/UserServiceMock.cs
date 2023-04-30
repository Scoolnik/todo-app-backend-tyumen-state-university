using TODOAppBackend.Entities;

namespace TODOAppBackend.Services;

public class UserServiceMock : IUserService
{
	private List<User> _users = new()
	{
		new User { Id = 1, Login = "string", Password = "string" }
	};

	public IEnumerable<User> GetAll() => _users;

	public User? GetById(int id) => _users.FirstOrDefault(x => x.Id == id);

	public User? GetByLogin(string? login) => _users.FirstOrDefault(x => x.Login == login);
}
