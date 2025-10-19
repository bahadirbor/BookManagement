using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Loan{
    public int Id { get; set; }

    public int BookId { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime LoanDate { get; set; }
    
    public DateTime? EstimatedReturnDate { get; set; }

    public bool IsReturned { get; set; }
}

class LoanConfiguration : IEntityTypeConfiguration<Loan>{
    public void Configure(EntityTypeBuilder<Loan> builder){
        builder.HasKey(l => l.Id);

        builder.Property(l => l.LoanDate)
               .IsRequired();

    }
}
