using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> GetByFilter(FilterModel filter);
        T Get(int id);

    }
}
