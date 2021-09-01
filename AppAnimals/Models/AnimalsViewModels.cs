using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Models
{
    public class AnimalsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string BirthDay { get; set; }
        public string Image { get; set; }
    }

    #region Animal Create
    public class AnimalCreateViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва тварини")]
        public string Name { get; set; }
        [Display(Name = "Дата народження")]
        public string BirthDay { get; set; }
        [Display(Name = "Фото")]
        public IFormFile Image { get; set; }
        [Display(Name = "Ціна тварини")]
        public decimal Price { get; set; }
    }

    public class AnimalCreateValidator : AbstractValidator<AnimalCreateViewModel>
    {
        public AnimalCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Поле не може бути порожнім");
            RuleFor(x => x.BirthDay).NotEmpty()
                .WithMessage("Поле не може бути порожнім");
            RuleFor(x => x.Price).NotEmpty()
                .WithMessage("Поле не може бути порожнім");
        }
    }
    #endregion

    #region Search Animal

    public class SearchHomeIndexModel
    {
        public string Name { get; set; }
    }

    public class HomeIndexModel
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public List<AnimalsViewModel> Animals { get; set; }
        public SearchHomeIndexModel Search { get; set; }
    }

    #endregion
}
