using KuzinShop.Models;
using KuzinShop.Models.DTO;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KuzinShop.Repositories
{
    public class ProductRepository: IRepository<ProductModel>
    {
        private readonly ApplicationContext _context;

        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<ProductModel> GetAll()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public List<ProductModel> GetByFilter(FilterModel filter)
        {
            IQueryable<ProductModel> products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                .ThenInclude(pa => pa.Attribute);

            // Фильтрация по основным параметрам
            if (!string.IsNullOrEmpty(filter.Name))
                products = products.Where(p => p.Name.Contains(filter.Name));

            if (filter.CategoryId.HasValue)
                products = products.Where(p => p.Category.Id == filter.CategoryId.Value);

            if (filter.MinPrice.HasValue)
                products = products.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= filter.MaxPrice.Value);

            if (!string.IsNullOrEmpty(filter.Publisher))
                products = products.Where(p => p.Publisher.Contains(filter.Publisher));

            if (filter.MinPlayersCount.HasValue)
                products = products.Where(p => p.PlayersCount >= filter.MinPlayersCount.Value);

            if (filter.MaxPlayersCount.HasValue)
                products = products.Where(p => p.PlayersCount <= filter.MaxPlayersCount.Value);

            // Фильтрация по строковым атрибутам
            foreach (var attributeFilter in filter.AttributeFilters)
            {
                if (attributeFilter.Key != null && attributeFilter.Value!=null)
                {
                    string attributeName = attributeFilter.Key.ToLower();
                    string value = attributeFilter.Value.ToLower();

                    if (value != null)
                        products = products.Where(p => p.ProductAttributes.Any(pa =>
                            pa.Attribute.Name.ToLower() == attributeName && pa.StringValue.ToLower().Contains(value)));
                }
            }

            foreach (var attributeFilter in filter.MinAttributeFilters)
            {
                if (attributeFilter.Key != null && attributeFilter.Value != null)
                {
                    string attributeName = attributeFilter.Key.ToLower();
                    int value = (int)attributeFilter.Value;

                    if (value != null)
                        products = products.Where(p => p.ProductAttributes.Any(pa =>
                            pa.Attribute.Name.ToLower() == attributeName && pa.IntegerValue >= value));
                }
            }

            foreach (var attributeFilter in filter.MaxAttributeFilters)
            {
                if (attributeFilter.Key != null && attributeFilter.Value != null)
                {
                    string attributeName = attributeFilter.Key.ToLower();
                    int value = (int)attributeFilter.Value;

                    if (value != null)
                        products = products.Where(p => p.ProductAttributes.Any(pa =>
                            pa.Attribute.Name.ToLower() == attributeName && pa.IntegerValue <= value));
                }

            }

            // Сортировка
            products = filter.SortBy switch
            {
                "Price" => filter.SortOrder == "asc" ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price),
                "Date" => filter.SortOrder == "asc" ? products.OrderBy(p => p.Date) : products.OrderByDescending(p => p.Date),
                _ => filter.SortOrder == "asc" ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name),
            };

            return products.ToList();
        }

        public ProductModel Get(int id)
        {
            return _context.Products.Include(p => p.Category).Include(p => p.ProductAttributes).ThenInclude(p => p.Attribute).FirstOrDefault(p => p.Id == id);
        }


    }
}
