namespace KuzinShop.Models.DTO
{
    public class ProductDetailDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } // Пк игры, настолки, спортивные
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string Publisher { get; set; }
        public int PlayersCount { get; set; }
        public string? Image { get; set; }

        public List<ProductDetailAttributesDTO> ProductAttributes { get; set; } = new List<ProductDetailAttributesDTO>();

    }
}
