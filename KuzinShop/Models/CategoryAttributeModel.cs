using System.ComponentModel.DataAnnotations.Schema;

namespace KuzinShop.Models
{
    public class CategoryAttributeModel
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public long AttributeId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CategoryModel Category { get; set; }

        [ForeignKey(nameof(AttributeId))]
        public AttributeModel Attribute { get; set; }
    }
}
