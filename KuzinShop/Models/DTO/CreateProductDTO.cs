﻿namespace KuzinShop.Models.DTO
{
    public class CreateProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int PlayersCount { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }

        public int CategoryId { get; set; }

        public List<CategoryModel> Categories { get; set; } = new();
        public List<AttributeDTO> Attributes { get; set; } = new();
    }

    public class AttributeDTO
    {
        public int AttributeId { get; set; }
        public string? StringValue { get; set; }
        public int? IntegerValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string DataType { get; set; } // Для определения типа данных на сервере
    }
}
