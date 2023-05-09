using TODOAppBackend.Entities;

namespace TODOAppBackend.Repository
{
    public interface IRepository<DBEntity> where DBEntity : BaseEntity
    {
        public DBEntity Create(DBEntity model);
        public DBEntity Update(DBEntity model);
        public void Delete(int id);
        public List<DBEntity> GetAll();
        public DBEntity Get(int id);
    }
}
