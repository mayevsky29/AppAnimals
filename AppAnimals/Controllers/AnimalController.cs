using AppAnimals.Domain;
using AppAnimals.Domain.Entities.Catalog;
using AppAnimals.Models;
using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppAnimals.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AppEFContext _context;
        private readonly IMapper _mapper;
        private IHostEnvironment _host;

        public AnimalController(AppEFContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            //GenataAnimal();
        }

        private void GenataAnimal()
        {
            int n = 50;
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 2,
                endDate.Month, endDate.Day);

            var faker = new Faker<Animal>("uk")
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.DateBirth, f => f.Date.Between(startDate, endDate))
                .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
                .RuleFor(x => x.Price, f => Decimal.Parse(f.Commerce.Price(100M, 500M)))
                .RuleFor(x => x.DateCreate, DateTime.Now);


            for (int i = 0; i < n; i++)
            {
                var animal = faker.Generate();
                _context.Animals.Add(animal);
                _context.SaveChanges();
            }
        }

        public IActionResult Index(SearchHomeIndexModel search, int page = 1)
        {
            int itemsCount = 5;

            var query = _context.Animals.AsQueryable();
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(search.Name.ToUpper()));
            }

            var models = query.Select(x =>
            _mapper.Map<AnimalsViewModel>(x))
                .Skip((page - 1) * itemsCount).Take(itemsCount).ToList();
            HomeIndexModel model = new HomeIndexModel();
            model.Animals = models;
            model.Search = search;
            model.Page = page;
            model.PageCount = (int)Math.Ceiling(query.Count() / (double)itemsCount);
            if (model.Page > model.PageCount && model.PageCount > 0)
                return RedirectToAction("Index", new { search.Name, page = model.PageCount });
            return View(model);
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AnimalCreateViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);
            string fileName = "";
            if (model.Image != null)
            {
                var ext = Path.GetExtension(model.Image.FileName);
                fileName = Path.GetRandomFileName() + ext;
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");

                var filePath = Path.Combine(dir, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }
            DateTime dt = DateTime.Parse(model.BirthDay, new CultureInfo("uk-UA"));
            Animal animal = new Animal
            {
                Name = model.Name,
                DateBirth = dt,
                Image = fileName,
                Price = model.Price,
                DateCreate = DateTime.Now
            };
            _context.Animals.Add(animal);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(long id)
        {
            Thread.Sleep(2000);
            var item = _context.Animals.SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                //_context.Remove(item);
                _context.Animals.Remove(item);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            AnimalCreateViewModel animal = new AnimalCreateViewModel();
            var editAnimal = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (editAnimal.Image != null)
            {
                var name = Path.GetFileName(editAnimal.Image);
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var filePath = Path.Combine(dir, name);

                using (var edit = System.IO.File.OpenRead($"{filePath}"))
                {
                    var newImage = new FormFile(edit, 0, edit.Length, null, Path.GetFileName(edit.Name));
                    animal.Name = editAnimal.Name;
                    animal.Price = editAnimal.Price;
                    animal.BirthDay = editAnimal.DateBirth.ToString();
                    animal.Image = newImage;
                }
            }
            return View(animal);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(long id, AnimalCreateViewModel model)
        {
            DateTime dt = DateTime.Parse(model.BirthDay, new CultureInfo("uk-UA"));
            if (ModelState.IsValid)
            {
                var editAnimal = _context.Animals.FirstOrDefault(a => a.Id == id);
                editAnimal.Name = model.Name;
                editAnimal.DateBirth = dt;
                editAnimal.Price = model.Price;
                string fileName = string.Empty;
                if (model.Image != null)
                {
                    var extension = Path.GetExtension(model.Image.FileName);
                    fileName = Path.GetRandomFileName() + extension;
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    var pathFile = Path.Combine(directory, fileName);
                    using (var stream = System.IO.File.Create(pathFile))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    var beforeImage = editAnimal.Image;
                    string folder = "\\images\\";
                    string path = _host.ContentRootPath + folder + beforeImage;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    editAnimal.Image = fileName;
                }
                _context.SaveChanges();
            };
            return RedirectToAction("Index");
        }

    }
}
