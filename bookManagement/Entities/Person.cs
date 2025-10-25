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
            .HasValue<Staff>("Staff")
            .HasValue<Member>("Member");

        builder.Property(b => b.FirstName)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(b => b.Surname)
               .IsRequired()
               .HasMaxLength(20);

        builder.HasData(new
        {
            PersonId = 1,
            FirstName = "Admin",
            Surname = "Admin",
            PersonType = "Administrator",
            Username = "admin",
            Password = 753159
        });

    }
} 
