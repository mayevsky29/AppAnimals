using AppAnimals.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Areas.Admin.Models
{
    public class UserViewModel
    {
        public CreateUserViewModel CreateUser { get; set; }
        public EditViewModel EditUser { get; set; }

        public AppEFContext context { get; set; }
        public string FormName { get; set; }

    }
}
