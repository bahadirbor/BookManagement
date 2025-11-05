using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Author{
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string WhoModifiedLast { get; set; } 

    public int NumberOfBooks 
    {
        get {return Books != null ? Books.Count : 0;}
    }

    public ICollection<Book> Books { get; set; }
}

class AuthorConfiguration : IEntityTypeConfiguration<Author>{

    public void Configure(EntityTypeBuilder<Author> builder){
        builder.HasKey(a => a.AuthorId);

        builder.Property(a => a.FirstName)
               .IsRequired()
               .HasMaxLength(25);

        builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(25);

        builder.Property(a => a.WhoModifiedLast)
                .IsRequired();
    }

}

class AuthorDto{
    [Required]
    [StringLength(25)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(25)]
    public string LastName { get; set; }
}

class AuthorOperations
{
    private readonly LibraryDbContext _context;

    public AuthorOperations()
    {
        _context = new LibraryDbContext();
    }

    public async Task ShowAllAuthorsAsync()
    {
        var authors = await _context.Authors.ToListAsync();

        foreach (var author in authors)
        {
            Console.WriteLine($"\nID: {author.AuthorId}");
            Console.WriteLine($"Name: {author.FirstName} {author.LastName}");
            Console.WriteLine(new string('-', 40));
        }
    }

    public async Task AddAuthorAsync(AuthorDto authorDto, Person person)
    {
        var author = new Author
        {
            FirstName = authorDto.FirstName,
            LastName = authorDto.LastName,
            WhoModifiedLast = person.Username
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        Console.WriteLine("Author added successfully.");
    }
    
    public async Task UpdateAuthorAsync(AuthorDto authorDto, Person person){
        var author = await _context.Authors.FirstOrDefaultAsync();
        
        if(author != null){
            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            author.WhoModifiedLast = person.Username;
            await _context.SaveChangesAsync();
            Console.WriteLine("Author updated successfully.");
        }
        else{
            Console.WriteLine("Author not found.");
        }
    }
    
    public async Task DeleteAuthorAsync(){
        Console.Write("Enter Author name to delete: ");
        string authorName = Console.ReadLine();
        string[] strings = authorName.Split(' ');

        //Name and surname are different fields 
        var author = await _context.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.FirstName == strings[0] && a.LastName == strings[1]);
        
        if(author != null){
            if(author.Books != null && author.Books.Count > 0){
                Console.WriteLine("Cannot delete author with associated books.");
                return;
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            Console.WriteLine("Author deleted successfully.");
        }
        else{
            Console.WriteLine("Author not found.");
        }
    }
}
