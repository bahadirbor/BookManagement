using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Category{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public ICollection<Book> Books { get; set; }
}

class CategoryConfiguration : IEntityTypeConfiguration<Category>{
    public void Configure(EntityTypeBuilder<Category> builder){
       builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(50);
    }

}