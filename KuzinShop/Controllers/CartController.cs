using KuzinShop.Models;
using KuzinShop.Repositories;
using KuzinShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KuzinShop.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly IProductRepository<ProductModel> _productRepository;

        public CartController(CartService cartService, IProductRepository<ProductModel> productRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cart = _cartService.CreateCart();
            if (cart.Products != null)
            {
                TempData["CartCount"] = cart.Products.Count();
            }
            TempData.Keep("CartCount");
            return View(cart);
        }

        public IActionResult Order(string name, string address, string phone)
        {
            var cart = _cartService.CreateCart();
            Order order = new Order();
            order.Name = name;
            order.Address = address;
            order.Phone = phone;
            order.Cart = cart;

            return View(order);

        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            var cart = _cartService.CreateCart();
            var product = _productRepository.Get(productId);

            if (cart.Products.FirstOrDefault(p => p.Product.Id == product.Id) != null)
                cart.IncreaseProduct(product);
            else cart.AddProduct(product);
            _cartService.UpdateCart(cart);
            return Redirect(Request.Headers["Referer"].ToString()); // Перенаправление на предыдущую страницу
        }

        [HttpPost]
        public IActionResult Increase(int productId)
        {
            var cart = _cartService.CreateCart();
            var product = _productRepository.Get(productId);

            cart.IncreaseProduct(product);
            _cartService.UpdateCart(cart);
            return Redirect(Request.Headers["Referer"].ToString()); // Перенаправление на предыдущую страницу
        }

        [HttpPost]
        public IActionResult Update(int productId, int count)
        {
            var cart = _cartService.CreateCart();
            var product = _productRepository.Get(productId);

            cart.UpdateProduct(product, count);
            _cartService.UpdateCart(cart);
            return Redirect(Request.Headers["Referer"].ToString()); // Перенаправление на предыдущую страницу
        }

        [HttpPost]
        public IActionResult Decrease(int productId)
        {
            var cart = _cartService.CreateCart();
            var product = _productRepository.Get(productId);

            cart.DecreaseProduct(product);
            _cartService.UpdateCart(cart);
            return Redirect(Request.Headers["Referer"].ToString()); // Перенаправление на предыдущую страницу
        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            var cart = _cartService.CreateCart();
            var product = _productRepository.Get(productId);

            cart.DeleteProduct(product);
            _cartService.UpdateCart(cart);
            return Redirect(Request.Headers["Referer"].ToString()); // Перенаправление на предыдущую страницу
        }
    }
}
