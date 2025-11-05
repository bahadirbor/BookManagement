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
    public string PublisherName { get; set; }
    public List<string> AuthorNameSurname { get; set; }
    public List<string> Categories { get; set; }
}

class BookOperations{
    //Book operations such as Show All Books
    private readonly LibraryDbContext _context;
    private readonly PersonOperations _personOperations;

    public BookOperations() {
        _context = new LibraryDbContext();
        _personOperations = new PersonOperations();
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

    public async Task AddBookAsync(BookDto dto, Person person)
    {
        // Implementation for adding a book

        var publisherId = await _context.Publishers.FirstOrDefaultAsync(a => a.Name == dto.PublisherName);
        
        if (publisherId == null)
        {
            Console.WriteLine($"Publisher {dto.PublisherName} not found.\nLet's add!");
            PublisherDto publisherDto = new PublisherDto();
            publisherDto.Name = dto.PublisherName;

            PublisherOperations publisherOperations = new PublisherOperations();
            await publisherOperations.AddPublisherAsync(publisherDto, person);
            publisherId = await _context.Publishers.FirstOrDefaultAsync(a => a.Name == dto.PublisherName);
        }

        Book book = new Book
        {
            BookName = dto.BookName,
            ISBN = dto.ISBN,
            PublicationDate = dto.PublicationDate,
            PublisherId = publisherId.PublisherId,
            IsLoaned = false,
            WhoModifiedLast = person.Username
        };

        publisherId.BookCount = (_context.Books.Where(b => b.PublisherId == publisherId.PublisherId).Count()) + 1;

        // Fetch and associate Authors
        foreach (var authorNameSurname in dto.AuthorNameSurname)
        {
            string authorName = authorNameSurname.Split(' ')[0];
            string authorSurname = authorNameSurname.Split(' ')[1];

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.FirstName == authorName && a.LastName == authorSurname);

            if (author != null)
            {
                book.Authors.Add(author);
            }
            else
            {
                Console.WriteLine($"Author with {authorName} {authorSurname} not found.\nLet's add!");
                AuthorDto authorDto = new AuthorDto();

                authorDto.FirstName = authorName;
                authorDto.LastName = authorSurname;

                AuthorOperations authorOperations = new AuthorOperations();
                await authorOperations.AddAuthorAsync(authorDto, person);

                var author2 = await _context.Authors.FirstOrDefaultAsync(a => a.FirstName == authorName && a.LastName == authorSurname);

                book.Authors.Add(author2);
            }

        }
        
        // Fetch and associate Categories
        foreach(var category in dto.Categories){
            var bookCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category);

            if(bookCategory != null){
                book.Categories.Add(bookCategory);
            }
            else {
                Console.WriteLine($"Category {category} not found.\nLet's add!");
                CategoryDto categoryDto = new CategoryDto();

                categoryDto.Name = category;

                CategoryOperations categoryOperations = new CategoryOperations();
                await categoryOperations.AddCategoryAsync(categoryDto, person);
            }
        }

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookAsync(BookDto dto, Person person){
        Console.Write("Enter the name of the book to update:");
        string bookName = Console.ReadLine();

        var book = await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Categories)
            .FirstOrDefaultAsync(b => b.BookName == bookName);

        if (book != null){
            Console.WriteLine("Book does not found!");
        }
        else{
            Console.WriteLine($"Book Name: {book.BookName}");
            Console.WriteLine($"ISBN: {book.ISBN}");
            Console.WriteLine($"Publication Date: {book.PublicationDate}");
            Console.WriteLine($"Publisher: {book.Publisher.Name}");
            Console.WriteLine($"Author: {book.Authors}");
            Console.WriteLine($"Categories: {book.Categories}");

            Console.Write("Select the field to update: ");
            string bookUpdateField = Console.ReadLine();
            
            Console.Write("Enter your password for verification: ");
            int Password = int.Parse(Console.ReadLine());

            if (_personOperations.Verify(person, Password))
            {
                BookDto bookDto = new BookDto();
                switch (bookUpdateField.ToLower())
                {
                    case "book name":
                        Console.Write("Enter new book name: ");
                        bookDto.BookName = Console.ReadLine();
                        book.BookName = bookDto.BookName;

                        await _context.SaveChangesAsync();
                        break;
                    case "isbn":
                        Console.Write("Enter new ISBN: ");
                        bookDto.ISBN = Console.ReadLine();
                        book.ISBN = bookDto.ISBN;

                        await _context.SaveChangesAsync();
                        break;
                    case "publication date":
                        Console.Write("Enter new publication date: ");
                        bookDto.PublicationDate = Console.ReadLine();
                        book.PublicationDate = bookDto.PublicationDate;
                        
                        await _context.SaveChangesAsync();
                        break;
                    case "publisher":
                        Console.Write("Enter new publisher name: ");
                        bookDto.PublisherName = Console.ReadLine();
                        var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Name == bookDto.PublisherName);
                        if (publisher != null)
                        {
                            book.PublisherId = publisher.PublisherId;
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            Console.WriteLine($"Publisher {bookDto.PublisherName} not found. Update operation aborted.");
                        }
                        break;
                    case "authors":
                        Console.Write("Enter new authors (comma separated): ");
                        string authorsInput = Console.ReadLine();
                        var authorNames = authorsInput.Split(',').Select(a => a.Trim()).ToList();
                        book.Authors.Clear();
                        foreach (var authorNameSurname in authorNames)
                        {
                            string authorName = authorNameSurname.Split(' ')[0];
                            string authorSurname = authorNameSurname.Split(' ')[1];
                            var author = await _context.Authors.FirstOrDefaultAsync(a => a.FirstName == authorName && a.LastName == authorSurname);
                            if (author != null)
                            {
                                book.Authors.Add(author);
                            }
                            else
                            {
                                Console.WriteLine($"Author with {authorName} {authorSurname} not found. Skipping.");
                            }
                        }
                        await _context.SaveChangesAsync();
                        break;
                    case "categories":
                        Console.Write("Enter new categories (comma separated): ");
                        string categoriesInput = Console.ReadLine();
                        var categoryNames = categoriesInput.Split(',').Select(c => c.Trim()).ToList();
                        book.Categories.Clear();
                        foreach (var categoryName in categoryNames)
                        {
                            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
                            if (category != null)
                            {
                                book.Categories.Add(category);
                            }
                            else
                            {
                                Console.WriteLine($"Category {categoryName} not found. Skipping.");
                            }
                        }
                        await _context.SaveChangesAsync();
                        break;
                    default:
                        break;
                }
            }
            else
                Console.WriteLine("Password is incorrect. Update operation aborted.");
        }
    }

    public async Task RemoveBookWithNameAsync(){
        Console.Write("Enter the book name to remove:");
        string bookName = Console.ReadLine();

        if (string.IsNullOrEmpty(bookName)){
            Console.WriteLine("Book name cannot be empty.");
        }
        else{
            var book = _context.Books.FirstOrDefault(b => b.BookName == bookName);

            if (book != null){
                var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.PublisherId == book.PublisherId);
                _context.Books.Remove(book);
                publisher.BookCount = (_context.Books.Where(b => b.PublisherId == publisher.PublisherId).Count()) - 1;
                await _context.SaveChangesAsync();
                Console.WriteLine($"Book '{bookName}' has been removed.");
            }
            else{
                Console.WriteLine($"Book '{bookName}' not found.");
            }
        }
    }

}