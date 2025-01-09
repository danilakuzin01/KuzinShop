namespace KuzinShop.Models
{
    public class CartModel
    {
        public List<CartItem> Products { get; set; } = new List<CartItem>();
        public int Sum { get; set; }


        public void AddProduct(ProductModel product)
        {
            CartItem item = new CartItem();
            item.Product = product;
            item.Count = 1;
            item.Sum = product.Price * item.Count;
            Products.Add(item);
        }

        public void IncreaseProduct(ProductModel product)
        {
            CartItem cartItem = Products.First(p => p.Product.Id == product.Id);

            cartItem.Count += 1;
            cartItem.Sum = product.Price * cartItem.Count;
        }

        public void UpdateProduct(ProductModel product, int count)
        {
            CartItem cartItem = Products.First(p => p.Product.Id == product.Id);

            cartItem.Count = count;
            cartItem.Sum = product.Price * cartItem.Count;
        }

        public void DecreaseProduct(ProductModel product)
        {
            CartItem cartItem = Products.First(p => p.Product.Id == product.Id);

            if (cartItem.Count > 1)
            {
                cartItem.Count -= 1;
                cartItem.Sum = product.Price * cartItem.Count;
            }
            else
                Products.Remove(cartItem);
        }

        public void DeleteProduct(ProductModel product)
        {
            CartItem cartItem = Products.First(p => p.Product.Id == product.Id);
            Products.Remove(cartItem);
        }

        public int GetSum()
        {
            Sum = 0;
            foreach (CartItem item in Products)
                Sum += item.Sum;
            return Sum;
        }
    }
}
