using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Book {
    
}

class BookConfiguration: IEntityTypeConfiguration<Book>{

    public void Configure(EntityTypeBuilder<Book> builder){
        
    }
}
