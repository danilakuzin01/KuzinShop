using KuzinShop.Models;

namespace KuzinShop.Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Delete(CategoryModel category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public CategoryModel Get(long id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public List<CategoryModel> GetAll()
        {
            return _context.Categories.ToList();
        }

        public List<CategoryModel> GetByFilter(FilterModel filter)
        {
            throw new NotImplementedException();
        }

        public void Save(CategoryModel category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(CategoryModel category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
