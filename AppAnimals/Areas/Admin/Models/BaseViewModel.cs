using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Areas.Admin.Models
{
    public abstract class BaseViewModel
    {
        [Display(Name = "Назва"),
            Required(ErrorMessage = "Поле не може бути пустим!")]
        public string Name { get; set; }
        [Display(Name = "Е-мейл"),
            Required(ErrorMessage = "Поле не може бути пустим!")
            , EmailAddress(ErrorMessage = "Пошта не валідна!")]
        public string Email { get; set; }
        [Display(Name = "Пароль"),
            DataType(DataType.Password)]
        public abstract string Password { get; set; }
        [Display(Name = "Фотографія")]
        public IFormFile Image { get; set; }
    }
}
