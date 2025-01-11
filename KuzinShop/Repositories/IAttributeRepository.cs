using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface IAttributeRepository<T> where T : class
    {
        List<ProductAttributeModel> GetAttributes(List<ProductModel> products);
        T Get(int id);

    }
}
