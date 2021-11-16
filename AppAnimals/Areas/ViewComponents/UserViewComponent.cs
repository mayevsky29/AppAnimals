using AppAnimals.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Areas.ViewComponents
{
    public class UserViewComponent : ViewComponent
    {
        private AppEFContext _context { get; set; }

        public UserViewComponent(AppEFContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync() 
        {
            return await Task.Run(() => { 
                var users =  _context.Users.ToList();
                return View("_UserCollection", users);
            });
        }
    }
}
