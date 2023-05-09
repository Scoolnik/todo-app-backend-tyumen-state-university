using TODOAppBackend.Entities;

namespace TODOAppBackend.Repository
{
    public class BaseRepository<DBEntity> : IRepository<DBEntity> where DBEntity : BaseEntity
    {
        private ApplicationContext Context { get; set; }
        public BaseRepository()
        {
            Context = new ApplicationContext();
        }

        public DBEntity Create(DBEntity model)
        {
            Context.Set<DBEntity>().Add(model);
            Context.SaveChanges();
            return model;
        }

        public void Delete(int id)
        {
            var toDelete = Context.Set<DBEntity>().FirstOrDefault(m => m.ID == id) 
                ?? throw new Exception("Элемент для удаления не найден");
            Context.Set<DBEntity>().Remove(toDelete);
            Context.SaveChanges();
        }

        public List<DBEntity> GetAll()
        {
            return Context.Set<DBEntity>().ToList();
        }

        public DBEntity Update(DBEntity model)
        {
            var toUpdate = Context.Set<DBEntity>().FirstOrDefault(m => m.ID == model.ID);
            if (toUpdate != null)
            {
                toUpdate = model;
            }
            Context.Update(toUpdate);
            Context.SaveChanges();
            return toUpdate;
        }

        public DBEntity Get(int id)
        {
            return Context.Set<DBEntity>().FirstOrDefault(m => m.ID == id)
                ?? throw new Exception("Элемент не найден");
        }
    }
}
