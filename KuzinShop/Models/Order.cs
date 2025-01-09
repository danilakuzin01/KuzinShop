namespace KuzinShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public CartModel Cart { get; set; }
    }
}
