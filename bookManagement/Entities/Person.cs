using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Person{
    public int PersonId { get; set; }

    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string PersonType { get; set; }

    public string Username { get; set; }

    public int Password { get; set; }
}

class PersonConfiguration : IEntityTypeConfiguration<Person>{
    public void Configure(EntityTypeBuilder<Person> builder){
        builder.HasKey(s => s.PersonId);

        builder.HasDiscriminator<string>("PersonType")
            .HasValue<Person>("Person")
            .HasValue<Staff>("Staff")
            .HasValue<Member>("Member");

        builder.Property(b => b.FirstName)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(b => b.Surname)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(p => p.Password)
            .IsRequired()
            .HasMaxLength(16);

        
        builder.HasData(new
        {
            PersonId = 1,
            FirstName = "Admin",
            Surname = "Admin",
            PersonType = "Person",
            Username = "admin",
            Password = 753159
        });

    }
}

class PersonOperations
{
    private readonly LibraryDbContext _context;

    public PersonOperations()
    {
        _context = new LibraryDbContext();
    }

    public async Task<Person?> LoginAsync(string username, int password)
    {
        var user = await _context.People.FirstOrDefaultAsync(p => p.Username == username && p.Password == password);

        if (user != null)
        {
            Console.WriteLine("Login successful!");
            return user;
        }
        else
        {
            Console.WriteLine("Invalid username or password. Please try again.");
            return null;
        }
    }
}