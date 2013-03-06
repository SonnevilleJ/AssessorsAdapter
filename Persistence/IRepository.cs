namespace Persistence
{
    public interface IRepository<T>
    {
        void Save(T obj);
        void Delete(T obj);
    }
}
