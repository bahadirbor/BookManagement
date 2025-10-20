using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Reservation{
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
}

class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(p => p.ReservationId);

        builder.Property(p => p.ReservationDate)
               .IsRequired();
    }
}
