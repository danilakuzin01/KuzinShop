using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface IAttributeRepository
    {
        List<ProductAttributeModel> GetAttributes(List<ProductModel> products);
    }
}
