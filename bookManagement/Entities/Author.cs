using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Author{
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Book> Books { get; set; }
    public int NumberOfBooks 
    {
        get {return Books != null ? Books.Count : 0;}
    }
}

class AuthorConfiguration : IEntityTypeConfiguration<Author>{

    public void Configure(EntityTypeBuilder<Author> builder){
        builder.HasKey(a => a.AuthorId);
    }

}