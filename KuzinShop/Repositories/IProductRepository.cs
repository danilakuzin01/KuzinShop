using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface IProductRepository<T> where T : class
    {   
        void Create(T product);
        List<T> GetAll();
        List<T> GetByFilter(FilterModel filter);
        T Get(int id);
        void Delete (ProductModel product);

    }
}
