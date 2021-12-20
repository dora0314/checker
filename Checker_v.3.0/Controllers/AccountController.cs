using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Checker_v._3._0.ViewModels;
using Checker_v._3._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Checker_v._3._0.Controllers
{
    public class AccountController : Controller
    {
        private DataContext dataContext;
        public AccountController(DataContext context)
        {
            dataContext = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Lk", "Users", new { userId = user.Id});
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await dataContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User 
                    { 
                        Email = model.Email, 
                        Password = model.Password,
                        Title = model.FullName,
                        ShortName = model.ShortName,
                        Role = UserRole.Student(dataContext)
                    };

                    dataContext.Users.Add(user);
                    await dataContext.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Lk", "Users", new { userId = user.Id });
                }
                else
                    ModelState.AddModelError("", "Такой пользователь уже зарегистрирован");
            }
            return View(model);
        }

        private async System.Threading.Tasks.Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, dataContext.UserRoles.Find(user.Role_id).Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
