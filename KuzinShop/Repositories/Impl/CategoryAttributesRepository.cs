using KuzinShop.Models;
using Microsoft.EntityFrameworkCore;

namespace KuzinShop.Repositories.Impl
{
    public class CategoryAttributesRepository
    {
        private readonly ApplicationContext _context;

        public CategoryAttributesRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<CategoryAttributeModel> GetAll()
        {
            return _context.CategoryAttributes.Include(ca => ca.Attribute).ToList();
        }

        public CategoryAttributeModel Get(int id)
        {
            return _context.CategoryAttributes.Include(ca => ca.Attribute).FirstOrDefault(ca => ca.Id == id);
        }

        public void Save(CategoryAttributeModel entity)
        {
            _context.CategoryAttributes.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CategoryAttributeModel entity)
        {
            _context.CategoryAttributes.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var entity = _context.CategoryAttributes.Find(id);
            if (entity != null)
            {
                _context.CategoryAttributes.Remove(entity);
                _context.SaveChanges();
            }
        }

        public List<AttributeModel> GetAttributesByCategoryId(long categoryId)
        {
            return _context.CategoryAttributes
                .Where(ca => ca.Category.Id == categoryId)
                .Include(ca => ca.Attribute)
                .Select(ca => ca.Attribute)
                .ToList();
        }

        public List<CategoryAttributeModel> GetAttributesByCategory(long categoryId)
        {
            return _context.CategoryAttributes
                .Include(ca => ca.Attribute) // Загрузка связанных данных
                .Where(ca => ca.Category.Id == categoryId)
                .ToList();
        }

        public void DeleteByAttributeId(long attributeId)
        {
            var categoryAttributes = _context.CategoryAttributes
                .Where(ca => ca.AttributeId == attributeId);
            _context.CategoryAttributes.RemoveRange(categoryAttributes);
            _context.SaveChanges();
        }
    }
}
