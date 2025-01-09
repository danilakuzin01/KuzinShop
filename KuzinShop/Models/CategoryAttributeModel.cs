namespace KuzinShop.Models
{
    public class CategoryAttributeModel
    {
        public int Id { get; set; }
        public CategoryModel Category { get; set; }
        public AttributeModel Attribute { get; set; }
    }
}
