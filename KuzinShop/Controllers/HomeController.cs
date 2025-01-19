using KuzinShop.Models;
using KuzinShop.Models.DTO;
using KuzinShop.Repositories;
using KuzinShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KuzinShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository<ProductModel> _productRepository;
        private readonly IRepository<CategoryModel> _categoryRepository;
        private readonly IAttributeRepository<AttributeModel> _attrributeRepository;
        private readonly CartService _cartService;
        private readonly ProductMapper _productMapper;

        public HomeController(ILogger<HomeController> logger, IProductRepository<ProductModel> productRepository, IRepository<CategoryModel> categoryRepository, CartService cartService, ProductMapper productMapper, IAttributeRepository<AttributeModel> attrributeRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _cartService = cartService;
            _productMapper = productMapper;
            _attrributeRepository = attrributeRepository;
        }


        [HttpGet]
        public IActionResult Index(FilterModel filter)
        {
            var cart = _cartService.CreateCart();
            if (cart.Products != null)
            {
                TempData["CartCount"] = cart.Products.Count();
            }
            TempData.Keep("CartCount");
            ViewBag.Cart = cart;

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", filter?.CategoryId);

            var products = filter != null
               ? _productRepository.GetByFilter(filter)
               : _productRepository.GetAll();

            List<ProductAttributeModel> productAttributes = _attrributeRepository.GetAttributes(products);
            ViewBag.Attributes = productAttributes;
            // Передаем текущий фильтр в представление
            ViewBag.Filter = filter;

            return View(products);
        }


        [HttpGet]
        public IActionResult Detail(int id)
        {
            var cart = _cartService.CreateCart();
            if (cart.Products != null)
            {
                TempData["CartCount"] = cart.Products.Count();
                ViewBag.ItemInCart = false;
                if (cart.Products.Count() > 0)
                {
                    if (cart.Products.Any(p => p.Product.Id == id))
                    {
                        ViewBag.ItemInCart = true;
                        ViewBag.ItemInCartCount = cart.Products.First(p => p.Product.Id == id).Count;
                    }
                }
            }
            TempData.Keep("CartCount");
            ProductModel product = _productRepository.Get(id);
            ProductDetailDTO productDetail = _productMapper.converToProductDetailDTO(product);

            return View(productDetail);
        }



        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
