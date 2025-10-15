using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Author{

}

class AuthorConfiguration : IEntityTypeConfiguration<Author>{

    public void Configure(EntityTypeBuilder<Author> builder)
    {
        
    }

}