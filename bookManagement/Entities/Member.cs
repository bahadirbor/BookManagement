using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Member{
    public int MemberId { get; set; }
    
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    
}

class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder){
        builder.HasKey(m => m.MemberId);

        builder.Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(30);
        
        builder.Property(m => m.Surname)
               .IsRequired()
               .HasMaxLength(30);
        
        builder.Property(m => m.Email)
               .IsRequired()
               .HasMaxLength(40);
        
        builder.Property(m => m.PhoneNumber)
               .HasMaxLength(15);
    }
}
