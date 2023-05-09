using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TODOAppBackend.Entities;
using TODOAppBackend.Repository;

namespace TODOAppBackend.Services;

public class UserService : IUserService
{
    private IRepository<TM_User> _repository;

	public UserService(IRepository<TM_User> repository)
    {
        _repository = repository;
    }

    public IEnumerable<TM_User> GetAll() => _repository.GetAll();

	public TM_User? GetById(int id) => _repository.Get(id);

	public TM_User? GetByLogin(string? login) => _repository.GetAll().FirstOrDefault(x => x.UserLogin == login);
}
