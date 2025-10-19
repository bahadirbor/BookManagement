using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Publisher{
    public int PublisherId { get; set; }

    public string Name { get; set; }

    public int? BookCount { get; set; }

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
