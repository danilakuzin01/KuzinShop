using KuzinShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace KuzinShop.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private CartModel _cart;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CartModel CreateCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            // Проверка, что сессия была корректно настроена
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }

            // Получение корзины из сессии
            if (session.Get("Cart") != null)
            {
                _cart = session.Get<CartModel>("Cart");
            }
            else
            {
                _cart = new CartModel();
            }

            // Сохранение корзины в сессии

            session.Set("Cart", _cart);
            return _cart;
        }

        public void UpdateCart(CartModel cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            // Проверка, что сессия была корректно настроена
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }

            // Сохранение корзины в сессии
            session.Set("Cart", cart);
        }
    }
}
