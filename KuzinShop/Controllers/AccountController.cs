using KuzinShop.Models;
using KuzinShop.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data;

namespace KuzinShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Login,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Назначение роли после успешного создания пользователя
                    var role = "User"; // Вы можете динамически задавать роль или использовать другое условие
                    var roleExist = await _roleManager.RoleExistsAsync(role);

                    if (!roleExist)
                    {
                        // Если роль не существует, создайте её
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }

                    // Добавление пользователя в роль
                    await _userManager.AddToRoleAsync(user, role);

                    // Вход пользователя в систему
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Перенаправление на главную страницу
                    return RedirectToAction("Index", "Home");
                }

                // Если произошли ошибки при создании пользователя, добавьте их в ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                // В случае ошибки верните пользователя на страницу входа
                return RedirectToAction("Login");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Login);
            if (user.IsActive == false)
            {
                return View(model);
            }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            ProfileViewModel model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Email,
                Address = user.Address
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Address = user.Address
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Обновление данных пользователя
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.Phone = model.Phone; 


            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Если нужно обновить пароль
            //if (!string.IsNullOrEmpty(model.NewPassword))
            //{
            //    var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            //    if (!passwordChangeResult.Succeeded)
            //    {
            //        foreach (var error in passwordChangeResult.Errors)
            //        {
            //            ModelState.AddModelError(string.Empty, error.Description);
            //        }
            //        return View(model);
            //    }
            //}

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Профиль успешно обновлен.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
