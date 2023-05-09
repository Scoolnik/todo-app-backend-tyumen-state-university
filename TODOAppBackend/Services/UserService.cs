using TODOAppBackend.Entities;
using TODOAppBackend.Repository;

namespace TODOAppBackend.Services;

public class UserService : IUserService
{
    private BaseRepository<TM_User> _repository;

	public UserService()
	{
		_repository = new BaseRepository<TM_User>();
	}

	public IEnumerable<TM_User> GetAll() => _repository.GetAll();

	public TM_User? GetById(int id) => _repository.Get(id);

	public TM_User? GetByLogin(string? login) => _repository.GetAll().FirstOrDefault(x => x.UserLogin == login);
}
