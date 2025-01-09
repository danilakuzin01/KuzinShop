namespace KuzinShop.Models.DTO
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public int PlayersCount { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }

        public int CategoryId { get; set; }

        public List<CategoryModel> Categories { get; set; } = new();
        public List<AttributeValueViewModel> Attributes { get; set; } = new();
    }

    public class AttributeValueViewModel
    {
        public int AttributeId { get; set; }
        public string Value { get; set; }
    }
}
