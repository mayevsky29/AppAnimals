using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DashboardsController : Controller
    {
        public IActionResult Dashboard_1()
        {
            return View();
        }

        public IActionResult Dashboard_2()
        {
            return View();
        }

        public IActionResult Dashboard_3()
        {
            return View();
        }

        public IActionResult Dashboard_4()
        {
            return View();
        }

        public IActionResult Dashboard_4_1()
        {
            return View();
        }

        public IActionResult Dashboard_5()
        {
            return View();
        }
    }
}
