using KuzinShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KuzinShop
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<AttributeModel> Attributes { get; set; }
        public DbSet<ProductAttributeModel> ProductAttributes { get; set; }
        public DbSet<CategoryAttributeModel> CategoryAttributes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
