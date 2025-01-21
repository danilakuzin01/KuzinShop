using KuzinShop.Controllers;
using KuzinShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KuzinShop.Repositories.Impl
{
    public class AttributeRepository : IAttributeRepository<AttributeModel>
    {
        private readonly ILogger<AttributeRepository> _logger;
        private readonly ApplicationContext _context;
        private List<ProductAttributeModel> attributes = new List<ProductAttributeModel>();

        public AttributeRepository(ILogger<AttributeRepository> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<ProductAttributeModel> GetAttributes(List<ProductModel> products)
        {
            // Получаем все атрибуты для переданных товаров
            var productIds = products.Select(p => p.Id).ToList();

            var attributes = _context.ProductAttributes
                .Include(pa => pa.Attribute)
                .Where(pa => productIds.Contains(pa.Product.Id))
                .ToList()
                .GroupBy(pa => new { AttributeName = pa.Attribute.Name })
                .Select(g => g.First())
                .ToList();

            return attributes;
        }

        public AttributeModel Get(long id)
        {
            return _context.Attributes.FirstOrDefault(a => a.Id == id); // Возвращаем первый найденный атрибут с указанным Id
        }

        public List<AttributeModel> GetAll()
        {
            return _context.Attributes.Include(p => p.CategoryAttributes).ThenInclude(p => p.Category).ToList();
        }

        public void Save(AttributeModel entity)
        {
            _context.Attributes.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            AttributeModel attribute = _context.Attributes.FirstOrDefault(p => p.Id == id);
            _context.Attributes.Remove(attribute);
            _context.SaveChanges();
        }

        public void Update(AttributeModel model)
        {
            _context.Attributes.Update(model);
            _context.SaveChanges();
        }

        public AttributeModel GetWithCategories(long id)
        {
            return _context.Attributes
                .Include(a => a.CategoryAttributes)
                .ThenInclude(ca => ca.Category)
                .FirstOrDefault(a => a.Id == id);
        }

    }
}
