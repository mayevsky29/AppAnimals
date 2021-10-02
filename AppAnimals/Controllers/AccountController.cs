using AppAnimals.Domain.Entities.Identity;
using AppAnimals.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager; // для роботи з користувачами
            _signInManager = signInManager; // для роботи з авторизацією
        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // 
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,
                        model.Password, false, false);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Дані вказано не коректно");
            return View(model);
        }


        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Така пошта уже є. Думай ...");
            }

            if (ModelState.IsValid)
            {
                user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Щось пішло не так.");
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
