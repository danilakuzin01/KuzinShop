using KuzinShop.Models;

namespace KuzinShop.Repositories
{
    public class CategoryRepository : IRepository<CategoryModel>
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public CategoryModel Get(int id)
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
    }
}
