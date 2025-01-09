using KuzinShop.Controllers;
using KuzinShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KuzinShop.Repositories
{
    public class AttributeRepository : IAttributeRepository
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
    }
}
