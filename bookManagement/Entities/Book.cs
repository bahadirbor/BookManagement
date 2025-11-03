using bookManagement.Data;
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
    public string WhoModifiedLast { get; set; }

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


class BookDto{
    //Created a Data Transfer Object for Book to facilitate data transfer
    public string BookName { get; set; }
    public string ISBN { get; set; }
    public string? PublicationDate { get; set; }
    public int PublisherId { get; set; }
    public List<string> AuthorNameSurname { get; set; }
    public List<int> Categories { get; set; }
}

class BookOperations{
    //Book operations such as Show All Books
    private readonly LibraryDbContext _context;
    
    public BookOperations() {
        _context = new LibraryDbContext();
    }

    public async Task ShowAllBooksAsync(){
        
        var books = await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .Include(b => b.Publisher)
            .ToListAsync();

        foreach(var book in books){
            Console.WriteLine($"Book ID: {book.BookId}");
            Console.WriteLine($"Book Name: {book.BookName}");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine($"Publication Date: {book.PublicationDate}");
            Console.WriteLine($"Publisher: {book.Publisher.Name}");
            Console.WriteLine($"Authors: {string.Join(", ", book.Authors.Select(a => a.FirstName + " " + a.LastName))}");
            Console.WriteLine($"Categories: {string.Join(", ", book.Categories.Select(c => c.Name))}");
            Console.WriteLine($"Added On: {book.AddDate}");
            Console.WriteLine($"Is Loaned: {(book.IsLoaned ? "Yes" : "No")}");
            Console.WriteLine(new string('-', 40));
        }
    }

    
}