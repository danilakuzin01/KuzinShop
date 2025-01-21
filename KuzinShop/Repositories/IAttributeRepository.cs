using KuzinShop.Models;
using Microsoft.EntityFrameworkCore;

namespace KuzinShop.Repositories
{
    public interface IAttributeRepository<T> where T : class
    {
        List<ProductAttributeModel> GetAttributes(List<ProductModel> products);
        List<AttributeModel> GetAll();
        T Get(long id);

        void Save(T entity);
        void Delete(long id);
        void Update(T model);

        public T GetWithCategories(long id);

    }
}
