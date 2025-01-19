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

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            // Получаем продукт из репозитория
            var product = _productRepository.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            // Формируем DTO
            var model = new CreateProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Publisher = product.Publisher,
                PlayersCount = product.PlayersCount,
                Image = product.Image,
                Price = product.Price,
                CategoryId = product.Category.Id,
                Categories = _categoryRepository.GetAll(),
                Attributes = _categoryAttributeRepository
                    .GetAttributesByCategoryId(product.Category.Id)
                    .Select(attr => new AttributeDTO
                    {
                        AttributeId = attr.Id,
                        Name = attr.Name,
                        DataType = attr.DataType,
                        StringValue = product.ProductAttributes
                            .FirstOrDefault(pa => pa.Attribute.Id == attr.Id)?.StringValue,
                        IntegerValue = product.ProductAttributes
                            .FirstOrDefault(pa => pa.Attribute.Id == attr.Id)?.IntegerValue,
                        DateValue = product.ProductAttributes
                            .FirstOrDefault(pa => pa.Attribute.Id == attr.Id)?.DateValue,
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(CreateProductDTO model)
        {
            // Получить существующий продукт
            var product = _productRepository.Get(model.Id);
            if (product == null)
            {
                return NotFound();
            }

            // Обновить основные свойства продукта
            product.Name = model.Name;
            product.Description = model.Description;
            product.Publisher = model.Publisher;
            product.PlayersCount = model.PlayersCount;
            product.Image = model.Image;
            product.Price = model.Price;

            // Обновить категорию
            var category = _categoryRepository.Get(model.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError("CategoryId", "Категория не найдена.");
                model.Categories = _categoryRepository.GetAll();
                return View(model);
            }
            product.Category = category;

            // Обновить атрибуты
            // Обрабатываем динамические атрибуты
            product.ProductAttributes = model.Attributes.Select(attr => new ProductAttributeModel
            {
                Attribute = _attributeRepository.Get(attr.AttributeId),
                StringValue = attr.DataType == "string" ? attr.StringValue : null,
                IntegerValue = attr.DataType == "int" ? attr.IntegerValue : null,
                DateValue = attr.DataType == "date" ? attr.DateValue : null
            }).ToList();

            // Сохранить изменения
            _productRepository.Update(product);

            // Перенаправить на список продуктов или другую страницу
            return RedirectToAction("Index");
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
            //List<User> users = use
            return View();
        }
    }
}
