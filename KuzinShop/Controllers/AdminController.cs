using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KuzinShop.Models;
using KuzinShop.Repositories;
using KuzinShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KuzinShop.Models.DTO;

namespace KuzinShop.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly CartService _cartService;
        private readonly IRepository<ProductModel> _productRepository;
        private readonly CategoryAttributesRepository _categoryAttributeRepository;
        private readonly IRepository<CategoryModel> _categoryRepository;

        public AdminController(CartService cartService, IRepository<ProductModel> productRepository, CategoryAttributesRepository categoryAttributes, IRepository<CategoryModel> categoryRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
            _categoryAttributeRepository = categoryAttributes;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartService.CreateCart();
            if (cart.Products != null)
            {
                TempData["CartCount"] = cart.Products.Count();
            }
            TempData.Keep("CartCount");

            List<ProductModel> products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Products()
        {
            List<ProductModel> products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult CreateProduct()
        {
            CreateProductDTO product = new CreateProductDTO {
                Categories = _categoryRepository.GetAll()
            };
            return View(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                // Получаем категорию продукта
                var category = _categoryRepository.Get(model.CategoryId);
                if (category == null)
                {
                    ModelState.AddModelError("", "Указанная категория не найдена.");
                    return View(model);
                }

                // Создаем объект продукта
                var product = new ProductModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Publisher = model.Publisher,
                    PlayersCount = model.PlayersCount,
                    Date = DateTime.Now,
                    Image = model.Image,
                    Category = category
                };

                // Добавляем атрибуты продукта
                //foreach (var attr in model.Attributes)
                //{
                //    var attribute = _categoryAttributeRepository.Get(attr.AttributeId);
                //    if (attribute != null)
                //    {
                //        product.ProductAttributes.Add(new ProductAttributeModel
                //        {
                //            Attribute = attribute.Attribute,
                //            Value = attr.Value
                //        });
                //    }
                //}

                // Сохраняем продукт в базе данных
                //_categoryAttributeRepository.Add(product);

                // Перенаправляем на список продуктов
                //return RedirectToAction("Products");
            }

            // Если данные некорректны, возвращаемся на страницу создания
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(model);
        }


        public IActionResult UpdateProduct(int id)
        {
            ProductModel productModel = _productRepository.Get(id);
            return View(productModel);
        }

        [HttpGet]
        public IActionResult GetCategoryAttributes(int categoryId)
        {
            var attributes = _categoryAttributeRepository
                .GetAttributesByCategoryId(categoryId)
                .Select(attr => new
                {
                    attr.Id,
                    attr.Name,
                    attr.Measure,
                    attr.DataType
                })
                .ToList();

            if (attributes == null || !attributes.Any())
            {
                return NotFound("Атрибуты для данной категории не найдены.");
            }
            
            return Json(attributes); // Это будет правильный массив объектов
        }


        public IActionResult Users()
        {

            return View();
        }
    }
}
