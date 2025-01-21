using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public interface IProductRepository: IRepository<ProductModel>
    {   
        void Save(ProductModel product);
        void Update(ProductModel product);
        void Delete (ProductModel product);
        List<ProductModel> GetByFilter(FilterModel filter);

    }
}
