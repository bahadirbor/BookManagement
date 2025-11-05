using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Category{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string WhoModifiedLast { get; set; }
    public ICollection<Book> Books { get; set; }
}

class CategoryConfiguration : IEntityTypeConfiguration<Category>{
    public void Configure(EntityTypeBuilder<Category> builder){
       builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(50);
    }

}

class CategoryDto{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
}

class CategoryOperations
{
    private readonly LibraryDbContext _context;
    public CategoryOperations()
    {
        _context = new LibraryDbContext();
    }
    
    public async Task ShowAllCategoriesAsync(){
        var categories = await _context.Categories.ToListAsync();

        foreach(var category in categories){
            Console.WriteLine($"Category ID: {category.CategoryId}, Name: {category.Name}, Last Modified By: {category.WhoModifiedLast}");
        }
    }

    public async Task AddCategoryAsync(CategoryDto categoryDto, Person person)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            WhoModifiedLast = person.Username
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        Console.WriteLine("Category added successfully.");
    }

    public async Task UpdateCategoryNameAsync(Person person){
        Console.Write("Enter Category name to update: ");
        string categroryName = Console.ReadLine();

        CategoryDto dto = new CategoryDto();

        var category = await _context.Categories.FirstOrDefaultAsync(a => a.Name == categroryName);
        if (category != null)
        {
            Console.Write("Enter new Category name: ");
            dto.Name = Console.ReadLine();

            category.Name = dto.Name;
            category.WhoModifiedLast = person.Username;
            
            await _context.SaveChangesAsync();
            Console.WriteLine("Category updated successfully.");
        }
        else
        {
            Console.WriteLine("Category not found.");
        }
    }

    public async Task RemoveCategoryWithNameAsync(string categoryName)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            Console.WriteLine("Category removed successfully.");
        }
        else
        {
            Console.WriteLine("Category not found.");
        }
    }

}