namespace KuzinShop.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public CartModel Cart { get; set; }
    }
}
