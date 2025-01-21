using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(long id);
    }
}
