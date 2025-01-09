namespace KuzinShop.Models.DTO
{
    public class ProductDetailAttributesDTO
    {
        public int Id { get; set; }
        public AttributeModel Attribute { get; set; }
        public string? Value { get; set; }

        public string? Measure { get; set; }
    }
}
