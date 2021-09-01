using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimals.Domain.Entities.Catalog
{
    public class Animal : BaseEntity<long>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DateBirth { get; set; }
        public string Image { get; set; }
    }
}
