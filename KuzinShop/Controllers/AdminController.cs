using KuzinShop.Models;
using KuzinShop.Models.DTO;
using KuzinShop.Models.ViewModels;
using KuzinShop.Repositories;
using KuzinShop.Repositories.Impl;
using KuzinShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KuzinShop.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly CartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly CategoryAttributesRepository _categoryAttributeRepository;
        private readonly UserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAttributeRepository<AttributeModel> _attributeRepository;

        public AdminController(UserManager<User> userManager, CartService cartService, IProductRepository productRepository, CategoryAttributesRepository categoryAttributes, UserRepository userRepository, ICategoryRepository categoryRepository, IAttributeRepository<AttributeModel> attributeRepository)
        {
            _userManager = userManager;
            _cartService = cartService;
            _productRepository = productRepository;
            _categoryAttributeRepository = categoryAttributes;
            _userRepository = userRepository;
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
            CreateProductDTO product = new CreateProductDTO
            {
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

                _productRepository.Save(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        [HttpGet]
        public IActionResult EditProduct(long id)
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


        public IActionResult DeleteProduct(long id)
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
        public IActionResult GetCategoryAttributes(long categoryId)
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


        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();

            var usersWithRoles = new List<AdminUsersViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new AdminUsersViewModel
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    UserName = user.UserName,
                    Roles = roles
                });
            }

            return View(usersWithRoles);
        }

        [HttpPost]
        public IActionResult ChangeUserStatus(string id)
        {
            User user = _userRepository.Get(id);
            _userRepository.ChangeUserStatus(user);


            return RedirectToAction("Users");
        }


        [HttpGet]
        public IActionResult Categories()
        {
            List<CategoryModel> categories = _categoryRepository.GetAll();
            return View(categories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel category)
        {
            _categoryRepository.Save(category);
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            CategoryModel category = _categoryRepository.Get(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel category)
        {
            _categoryRepository.Update(category);
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public IActionResult DeleteCategory(long id)
        {
            _categoryRepository.Delete(_categoryRepository.Get(id));
            return RedirectToAction("Categories");
        }


        [HttpGet]
        public IActionResult Attributes()
        {
            List<AttributeModel> attributes = _attributeRepository.GetAll();
            return View(attributes);
        }

        [HttpGet]
        public IActionResult CreateAttribute(long id)
        {
            CreateAttributeViewModel attributeViewModel = new CreateAttributeViewModel();
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(attributeViewModel);
        }

        [HttpPost]
        public IActionResult CreateAttribute(CreateAttributeViewModel model)
        {
            // Создаем новый атрибут и связываем его с категорией
            var attribute = new AttributeModel
            {
                Name = model.Name,
                DataType = model.DataType,
                Measure = model.Measure
            };

            _attributeRepository.Save(attribute);

            var categoryAttribute = new CategoryAttributeModel
            {
                AttributeId = attribute.Id,
                CategoryId = model.Category.Id // Связь с категорией
            };

            _categoryAttributeRepository.Save(categoryAttribute);

            return RedirectToAction("Attributes");
        }

        [HttpGet]
        public IActionResult EditAttribute(long id)
        {
            // Получаем атрибут и связанные категории
            var attribute = _attributeRepository.GetWithCategories(id);
            if (attribute == null)
            {
                return NotFound("Attribute not found");
            }

            // Заполняем модель
            var model = new EditAttributeViewModel
            {
                Id = attribute.Id,
                Name = attribute.Name,
                DataType = attribute.DataType,
                Measure = attribute.Measure,
                SelectedCategoryIds = attribute.CategoryAttributes.Select(ca => ca.CategoryId).ToList(),
                Categories = _categoryRepository.GetAll().ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateAttribute(EditAttributeViewModel model)
        {
            // Получаем атрибут из базы
            var attribute = _attributeRepository.GetWithCategories(model.Id);
            if (attribute == null)
            {
                return NotFound("Attribute not found");
            }

            // Обновляем данные атрибута
            attribute.Name = model.Name;
            attribute.DataType = model.DataType;
            attribute.Measure = model.Measure;

            // Удаляем старые связи с категориями
            _categoryAttributeRepository.DeleteByAttributeId(model.Id);

            // Добавляем новые связи с категориями
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                var category = _categoryRepository.Get(categoryId);
                if (category != null)
                {
                    _categoryAttributeRepository.Save(new CategoryAttributeModel
                    {
                        AttributeId = model.Id,
                        CategoryId = categoryId
                    });
                }
            }

            // Сохраняем изменения
            _attributeRepository.Update(attribute);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteAttribute(long id)
        {
            _attributeRepository.Delete(id);
            return RedirectToAction("Attributes");
        }
    }
}
