using System.Text.Json.Serialization;

namespace KuzinShop.Models
{
    public class ProductAttributeModel
    {
        public long Id { get; set; }

        [JsonIgnore] // Пропускаем циклическую ссылку на Product
        public ProductModel Product { get; set; }
        public AttributeModel Attribute { get; set; }
        public string? StringValue { get; set; }
        public int? IntegerValue { get; set; }
        public DateTime? DateValue { get; set; }
    }
}
