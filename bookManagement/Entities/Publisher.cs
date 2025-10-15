using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Publisher{
}

class PublisherConfiguration : IEntityTypeConfiguration<Publisher>{

    public void Configure(EntityTypeBuilder<Publisher> builder)
    {

    }
}
