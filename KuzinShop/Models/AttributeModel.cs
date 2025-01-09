namespace KuzinShop.Models
{
    public class AttributeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? DataType { get; set; }
        public string? Measure { get; set; }

        public List<CategoryAttributeModel> CategoryAttributes { get; set; } = new();
    }
}
