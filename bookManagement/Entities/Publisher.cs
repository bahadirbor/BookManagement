using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Publisher{
    public int PublisherId { get; set; }

    public string Name { get; set; }

    public int? BookCount { get; set; }

    public ICollection<Book> Books { get; set; }

}

class PublisherConfiguration : IEntityTypeConfiguration<Publisher>{

    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(p => p.PublisherId);

        builder.Property(p => p.Name)
           .IsRequired()
           .HasMaxLength(30);
    }
}

class PublisherDto{
    [Required]
    [StringLength(30)]
    public string Name { get; set; }
}

class PublisherOperations
{
    private readonly LibraryDbContext _context;
    public PublisherOperations()
    {
        _context = new LibraryDbContext();
    }

    public async Task ShowAllPublishersAsync()
    {
        List<Publisher> publishers = await _context.Publishers.ToListAsync();
        Console.WriteLine("Publishers:");

        foreach (var publisher in publishers)
        {
            Console.WriteLine($"- {publisher.Name}");
        }
    }

    public async Task AddPublisherAsync(PublisherDto publisherDto, Person person)
    {
        var publisher = new Publisher
        {
            Name = publisherDto.Name
        };

        await _context.Publishers.AddAsync(publisher);
        await _context.SaveChangesAsync();
        Console.WriteLine("Publisher added successfully.");
    }

    public async Task UpdatePublisherAsync(PublisherDto publisherDto, Person person){
        Console.WriteLine("Enter the name of the publisher to update:");
        string oldName = Console.ReadLine();

        var publisher = await _context.Publishers.FirstOrDefaultAsync(p => p.Name == oldName);

        if (publisher == null)
        {
            Console.WriteLine("Publisher not found.");
        }
        else
        {
            Console.WriteLine("Enter the new name for the publisher:");
            publisherDto.Name = Console.ReadLine();
            publisher.Name = publisherDto.Name;

            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();
            Console.WriteLine("Publisher updated successfully.");
        }
    }

    public async Task RemovePublisherAsync(string publisherName){
        var publisher = await _context.Publishers.FirstOrDefaultAsync(c => c.Name == publisherName);
        if (publisher != null)
        {
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            Console.WriteLine("Category removed successfully.");
        }
        else
        {
            Console.WriteLine("Category not found.");
        }
    }

}