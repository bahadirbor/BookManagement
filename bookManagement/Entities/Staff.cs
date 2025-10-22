using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Staff{
    public int StaffId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
    public string StaffUsername { get; set;}
    public int StaffPassword { get; set; }
}

class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder){
        builder.HasKey(s => s.StaffId);
        
        builder.Property(b => b.FirstName)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(b => b.Surname)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(b => b.Position)
            .IsRequired();

        builder.Property(b => b.StaffPassword)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasData(new
        {
            StaffId = 1,
            FirstName = "Admin",
            Surname = "Admin",
            Position = "Administrator",
            StaffUsername = "admin",
            StaffPassword = 753159
        });
    }
}