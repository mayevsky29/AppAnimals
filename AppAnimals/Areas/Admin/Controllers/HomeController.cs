using AppAnimals.Areas.Admin.Models;
using AppAnimals.Constants;
using AppAnimals.Domain;
using AppAnimals.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/{controller=Home}/{action=Index}")]
    public class HomeController : Controller
    {
        private UserManager<AppUser> _userManager { get; set; }
        private AppEFContext _context { get; set; }
        private RoleManager<AppRole> _roleManager { get; set; }
        public HomeController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppEFContext context)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            UserViewModel userModel = new UserViewModel();
            userModel.FormName = "adduser";
            userModel.context = _context;

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            UserViewModel ErrorModel = new UserViewModel { 
                CreateUser = model,
                context = _context,
                FormName = "adduser"
            };
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введено не правильно!");
                return View("Index", ErrorModel);
            }
            string fileName = "";
            if (model.Image != null)
            {
                fileName = System.IO.Path.GetRandomFileName() +
                    System.IO.Path.GetExtension(model.Image.FileName);

                string dirPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "images");

                using (var file = System.IO.File.Create(Path.Combine(dirPath, fileName)))
                {
                    model.Image.CopyTo(file);
                }
            }

            var appuser = new AppUser
            {
                Name = model.Name,
                UserName = model.Email,
                Email = model.Email,
                Image = fileName
            };


            var result = await _userManager.CreateAsync(appuser, model.Password);

            _userManager.AddToRoleAsync(appuser, Roles.User).Wait();
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Під час реєстрації виникла помилка!");
                return View("Index", ErrorModel);
            }

            return Redirect("/Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditViewModel model, string EmailOld) 
        {
            UserViewModel ErrorModel = new UserViewModel {
                EditUser = model,
                context = _context,
                FormName = "edituser"
            };
            if (!ModelState.IsValid) 
            {
                ModelState.AddModelError("", "Дані введено не коректно!");
                return View("Index", ErrorModel);
            }
            AppUser user = await _userManager.FindByNameAsync(EmailOld);
            if (user != null)
            {
                
                user.Email = model.Email;
                user.Name = model.Name;
                user.UserName = model.Email;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    string hashPass = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    user.PasswordHash = hashPass;
                }

                if (model.Image != null)
                {
                    if (!string.IsNullOrEmpty(user.Image))
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "images", user.Image);
                        System.IO.File.Delete(imagePath);
                    }

                    string fileName = System.IO.Path.GetRandomFileName() +
                            System.IO.Path.GetExtension(model.Image.FileName);

                    string dirPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "images");

                    using (var file = System.IO.File.Create(Path.Combine(dirPath, fileName)))
                    {
                        model.Image.CopyTo(file);
                    }
                    user.Image = fileName;
                }

                await _userManager.UpdateAsync(user);
                //_context.SaveChanges();
            }
            else 
            {
                ModelState.AddModelError("", "Щось пішло не так!");
                return View("Index", ErrorModel);
            }

            return Redirect("/Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<string> options) 
        {
            foreach (var option in options) 
            {
                var user = await _userManager.FindByNameAsync(option);
                if (user != null) 
                {
                  await _userManager.DeleteAsync(user);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", user.Image);
                    if (System.IO.File.Exists(filePath)) 
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            return Redirect("/Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string email, string role) 
        {
            AppUser user = await _userManager.FindByNameAsync(email);
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == user.Id));
            if (user != null) 
            {
                switch (role) 
                {
                    case "isUser": 
                        {
                            await _userManager.AddToRoleAsync(user, Roles.User);
                            break; 
                        }
                    case "isVip": 
                        {
                            await _userManager.AddToRoleAsync(user, Roles.Manager);
                            break; 
                        }
                    case "isAdmin": 
                        {
                            await _userManager.AddToRoleAsync(user, Roles.Admin);
                            break; 
                        }
                }
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRole(string email)
        {
            AppUser user = await _userManager.FindByNameAsync(email);
            
            var role = _context.UserRoles.Where(x => x.UserId == user.Id).ToList().FirstOrDefault();
            string roleName = _roleManager.FindByIdAsync(role.RoleId.ToString()).Result.Name;
            return Ok(JsonConvert.SerializeObject(roleName));
        }

        [HttpGet]
        public IActionResult GetData(string email) 
        {
            return Ok(JsonConvert.SerializeObject(_context.Users.FirstOrDefault(x => x.Email == email)));
        }

        [HttpPost]
        public IActionResult ChangeImage(ChangeImageModel obj) 
        {

            AppUser user = _userManager.FindByNameAsync(obj.email).Result;
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.Image)) 
                {
                    System.IO.File.Delete(user.Image);
                }
                byte[] bytesImage = Convert.FromBase64String(obj.img.Split(',')[1]);

                MemoryStream ms = new MemoryStream(bytesImage);
                IFormFile file = new FormFile(ms, 0, bytesImage.Length, "", "");
                string ext = "";
                if (obj.img.Contains("jpeg"))
                    ext = ".jpeg";
                else if (obj.img.Contains("jpg"))
                    ext = ".jpg";
                else if (obj.img.Contains("png"))
                    ext = ".png";

                if (!string.IsNullOrEmpty(ext))
                {
                    string fileName = Path.GetRandomFileName() + ext;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }


                    user.Image = fileName;

                    _userManager.UpdateAsync(user).Wait();
                }
            }

            UserViewModel userModel = new UserViewModel();
            userModel.FormName = "editimage";
            userModel.context = _context;
            return View("Index", userModel);
        }
    }
}
