using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
        void Save(CategoryModel category);
        void Update(CategoryModel category);
        void Delete(CategoryModel category);
    }
}
