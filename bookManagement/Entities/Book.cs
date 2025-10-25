using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Book {
    public int Id { get; set; }
    public string BookName { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public int PublisherId { get; set; }
    public DateTime AddDate { get; set; }
    public ICollection<Author> Authors { get; set; }
    public ICollection<Category> Categories { get; set; }
}

class BookConfiguration: IEntityTypeConfiguration<Book>{

    public void Configure(EntityTypeBuilder<Book> builder){
        builder.HasKey(b => b.Id);

        builder.HasAlternateKey(b => b.ISBN);

        //Many-to-Many relationship between Book and Author has been configured
        builder.HasMany<Author>()
            .WithMany(a => a.Books)
            .UsingEntity(j => j.ToTable("BookAuthors"));

        //Many-to-Many relationship between Book and Category has been configured
        builder.HasMany<Category>()
            .WithMany(c => c.Books)
            .UsingEntity(j => j.ToTable("BookCategories"));

        //One-to-Many relationship between Book and Publisher has been configured
        builder.HasOne<Publisher>()
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(b => b.BookName)
             .IsRequired()
             .HasMaxLength(100);

        //Implemented AddDate property with book addition timestamp
        builder.Property(d => d.AddDate)
               .HasDefaultValueSql("GETDATE()");

    }
}
