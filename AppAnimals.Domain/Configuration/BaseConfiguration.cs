using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimals.Domain.Configuration
{
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
        public void CreateDefaultConfiguration(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey("Id");

            builder.Property("DateCreate")
                .IsRequired();
        }
    }
}
