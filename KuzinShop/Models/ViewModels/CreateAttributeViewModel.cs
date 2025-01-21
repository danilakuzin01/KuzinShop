namespace KuzinShop.Models.ViewModels
{
    public class CreateAttributeViewModel
    {
        public string Name { get; set; }
        public string? DataType { get; set; }
        public string? Measure { get; set; }

        public CategoryModel Category { get; set; }
    }
}
