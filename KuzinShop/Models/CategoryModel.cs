using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KuzinShop.Models
{
    public class CategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Указывает автоинкремент
        public long Id { get; set; }
        public string Name { get; set; }


        [JsonIgnore] // Пропускаем при сериализации, чтобы избежать цикла
        public List<ProductModel> Products { get; set; } = new();
        [JsonIgnore] // Пропускаем при сериализации, чтобы избежать цикла
        public List<CategoryAttributeModel> CategoryAttributes { get; set; } = new();
    }
}
