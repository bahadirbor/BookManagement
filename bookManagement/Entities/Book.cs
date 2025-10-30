using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Book {
    public int BookId { get; set; }
    public string BookName { get; set; }
    public string ISBN { get; set; }
    public string? PublicationDate { get; set; }
    public int PublisherId { get; set; }
    public DateTime AddDate { get; set; }
    public bool IsLoaned { get; set; }

    //Navigation Properties
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public Publisher Publisher { get; set; } = null!;
}

class BookConfiguration: IEntityTypeConfiguration<Book>{

    public void Configure(EntityTypeBuilder<Book> builder){
        builder.HasKey(b => b.BookId);

        builder.HasAlternateKey(b => b.ISBN);
        
        //Many-to-Many relationship between Book and Author has been configured
        builder.HasMany(a => a.Authors)
            .WithMany(a => a.Books)
            .UsingEntity(j => j.ToTable("BookAuthors"));
        
        //Many-to-Many relationship between Book and Category has been configured
        builder.HasMany(a => a.Categories)
            .WithMany(c => c.Books)
            .UsingEntity(j => j.ToTable("BookCategories"));
        
        //One-to-Many relationship between Book and Publisher has been configured
        builder.HasOne(a => a.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId);

        builder.Property(b => b.BookName)
             .IsRequired()
             .HasMaxLength(100);
        
        builder.Property(b => b.IsLoaned)
            .IsRequired();
        
        //Implemented AddDate property with book addition timestamp
        builder.Property(d => d.AddDate)
            .HasDefaultValueSql("GETDATE()");
        
    }
}
