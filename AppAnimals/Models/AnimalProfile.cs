using AppAnimals.Domain.Entities.Catalog;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AppAnimals.Models
{
    public class AnimalProfile : Profile
    {
        public AnimalProfile()
        {
            CreateMap<Animal, AnimalsViewModel>()
                .ForMember(x => x.BirthDay,
                            opt => opt.MapFrom(x => x.DateBirth
                            .ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("uk"))
                            ));
            //.ReverseMap();
            //CreateMap<Animal, AnimalsViewModel>();

        }
    }
}
