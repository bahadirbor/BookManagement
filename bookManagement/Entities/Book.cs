using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Book {
    public int BookId { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public int PublicationYear { get; set; }
    public int PublisherId { get; set; }
    public ICollection<Author> Authors { get; set; }
    public ICollection<Category> Categories { get; set; }
}

class BookConfiguration: IEntityTypeConfiguration<Book>{

    public void Configure(EntityTypeBuilder<Book> builder){
        builder.HasKey(b => b.BookId);

    }
}
