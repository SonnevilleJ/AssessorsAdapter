namespace Persistence
{
    public interface IRepository<TKey, TValue>
    {
        void Save(TKey key, TValue value);
        void Delete(TKey key);
    }
}
