using AppAnimals.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimals.Domain.Configuration.Catalog
{
    public class AnimalConfiguration : BaseConfiguration<Animal>
    {
        public override void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.ToTable("tblAnimals");

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.Image)
                .HasMaxLength(255)
                .IsRequired(false);
        }
    }
}
