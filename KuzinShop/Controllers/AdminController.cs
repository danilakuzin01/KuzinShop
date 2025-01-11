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

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly CartService _cartService;
        private readonly IProductRepository<ProductModel> _productRepository;
        private readonly CategoryAttributesRepository _categoryAttributeRepository;
        private readonly IRepository<CategoryModel> _categoryRepository;
        private readonly IAttributeRepository<AttributeModel> _attributeRepository;

        public AdminController(CartService cartService, IProductRepository<ProductModel> productRepository, CategoryAttributesRepository categoryAttributes, IRepository<CategoryModel> categoryRepository, IAttributeRepository<AttributeModel> attributeRepository)
        {
            _cartService = cartService;
            _productRepository = productRepository;
            _categoryAttributeRepository = categoryAttributes;
            _categoryRepository = categoryRepository;
            _attributeRepository = attributeRepository;
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

        // Страница для создания продукта
        public IActionResult CreateProduct()
        {
            CreateProductDTO product = new CreateProductDTO {
                Categories = _categoryRepository.GetAll()
            };
            return View(product);
        }

        // Обработка данных для создания продукта
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(CreateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var product = new ProductModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Publisher = model.Publisher,
                    PlayersCount = model.PlayersCount,
                    Image = model.Image,
                    Price = model.Price,
                    Category = _categoryRepository.Get(model.CategoryId),
                    Date = DateTime.Now
                };

                // Обработка атрибутов
                product.ProductAttributes = model.Attributes.Select(attr => new ProductAttributeModel
                {
                    Attribute = _attributeRepository.Get(attr.AttributeId),
                    StringValue = attr.DataType == "string" ? attr.StringValue : null,
                    IntegerValue = attr.DataType == "int" ? attr.IntegerValue : null,
                    DateValue = attr.DataType == "date" ? attr.DateValue : null
                }).ToList();

                _productRepository.Create(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        public IActionResult EditProduct(int id)
        {
            ProductModel productModel = _productRepository.Get(id);


            CreateProductDTO product = new CreateProductDTO {
                Id = productModel.Id,
                Name = productModel.Name,
                Description = productModel.Description,
                Publisher = productModel.Publisher,
                Price = productModel.Price,
                PlayersCount = productModel.PlayersCount,
                Image = productModel.Image,
                CategoryId = productModel.Category?.Id ?? 0, // Обеспечиваем, чтобы CategoryId был корректным
                Categories = _categoryRepository.GetAll() // Заполняем список категорий
            };
            

            return View(product);
        }

        public IActionResult DeleteProduct(int id)
        {
            // Найти продукт по Id
            var product = _productRepository.Get(id);

            if (product == null)
            {
                // Если продукт не найден, возвращаем ошибку или страницу с уведомлением
                return NotFound();
            }

            // Удалить продукт
            _productRepository.Delete(product);

            return RedirectToAction("Index");
        }

        // Запрос на получение аттрибутов согласно выбранной категории
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
