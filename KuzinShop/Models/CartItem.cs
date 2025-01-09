namespace KuzinShop.Models
{
    public class CartItem
    {
        public ProductModel Product { get; set; }
        public int Count { get; set; }
        public int Sum { get; set; }
    }
}
