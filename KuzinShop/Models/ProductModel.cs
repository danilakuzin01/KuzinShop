using Microsoft.AspNetCore.Mvc;

namespace KuzinShop.Models
{
    public class ProductModel
    {
        public long Id { get; set; }
        public CategoryModel Category { get; set; } // Пк игры, настолки, спортивные
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string Publisher { get; set; }
        public int PlayersCount { get; set; }
        public string? Image { get; set; }

        public List<ProductAttributeModel> ProductAttributes { get; set; } = new();


        // Конструктор по умолчанию (требуется для EF Core)
        public ProductModel() { }

        public ProductModel(long id, CategoryModel category, string name, string description, DateTime date, int price, string publisher, int playersCount, string image)
        {
            Id = id;
            Category = category;
            Name = name;
            Description = description;
            Date = date;
            Price = price;
            Publisher = publisher;
            PlayersCount = playersCount;
            Image = image;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
