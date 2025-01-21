namespace KuzinShop.Models.ViewModels
{
    public class EditAttributeViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? DataType { get; set; }
        public string? Measure { get; set; }

        // Список выбранных категорий
        public List<long> SelectedCategoryIds { get; set; } = new();

        // Все доступные категории
        public List<CategoryModel> Categories { get; set; } = new();
    }
}
