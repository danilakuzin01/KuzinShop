namespace KuzinShop.Models.DTO
{
    public class CreateProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int PlayersCount { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }

        public long CategoryId { get; set; }

        public List<CategoryModel> Categories { get; set; } = new();
        public List<AttributeDTO> Attributes { get; set; } = new();
    }

    public class AttributeDTO
    {
        public long AttributeId { get; set; }
        public string Name { get; set; } // Название атрибута
        public string? StringValue { get; set; }
        public int? IntegerValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string DataType { get; set; } // Для определения типа данных на сервере
    }
}
