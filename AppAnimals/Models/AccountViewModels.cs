using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Електронна адреса")]
        [Required(ErrorMessage = "Вкажіть пошту")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Вкажіть пароль")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Обов'язкове поле")]
        [EmailAddress(ErrorMessage = "Не корекна пошта")]
        public string Email { get; set; }

        //
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Invalid password")]
        public string Password { get; set; }
    }

    public class UserNavbarInfoViewModel
    {
        public string FullName { get; set; }
        public string Image { get; set; }
    }
}
