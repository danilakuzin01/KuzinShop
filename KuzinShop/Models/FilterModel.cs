namespace KuzinShop.Models
{
    public class FilterModel
    {
        public string Name { get; set; } // Поиск по названию
        public long? CategoryId { get; set; } // Фильтрация по категории
        public int? MinPrice { get; set; } // Минимальная цена
        public int? MaxPrice { get; set; } // Максимальная цена
        public string Publisher { get; set; } // Издатель
        public int? MinPlayersCount { get; set; } // Минимальное количество игроков
        public int? MaxPlayersCount { get; set; } // Максимальное количество игроков


        // Добавляем параметры для сортировки
        public string SortBy { get; set; } = "Price";  // Поле для сортировки (по умолчанию по имени)
        public string SortOrder { get; set; } = "asc"; // Порядок сортировки (по умолчанию по возрастанию)

        // Фильтры по атрибутам товаров
        public Dictionary<string, string> AttributeFilters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, int?> MinAttributeFilters { get; set; } = new Dictionary<string, int?>();
        public Dictionary<string, int?> MaxAttributeFilters { get; set; } = new Dictionary<string, int?>();


    }
}
