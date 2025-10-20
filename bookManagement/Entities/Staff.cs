using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Staff{
    public int StaffId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
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
    }
}