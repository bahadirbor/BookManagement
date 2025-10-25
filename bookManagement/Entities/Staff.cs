using bookManagement.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Staff : Person{
    public string Position { get; set; }
}

class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder){
        builder.Property(b => b.Position)
            .IsRequired()
            .HasDefaultValue("Librarian");

        builder.Property(b => b.Password)
            .IsRequired()
            .HasMaxLength(10);
    }
}

class StaffDto{
    //Created a Data Transfer Object for Staff to facilitate data transfer
    [Required]
    [StringLength(20)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Surname { get; set; }

    [Required]
    [StringLength(20)]
    public int Password { get; set; }
}

class StaffOperations{
    //Staff operations such as Add, Update, Remove
    private readonly libraryDbContext _context;

    public StaffOperations() {
        _context = new libraryDbContext();
    }

    public async Task AddStaff(StaffDto dto){

        string username = (dto.FirstName[0] + dto.Surname).ToLower();

        Staff staff = new Staff() {
            FirstName = dto.FirstName,
            Surname = dto.Surname,
            Username = username,
            Password = dto.Password
        };

        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();

        Console.WriteLine("Staff added successfully.");
    }

    public void UpdateStaff(StaffDto dto){
        //
    }

    public async Task RemoveStaffWithUsername() {
        // Remove staff based on username
        Console.Write("Enter the username of the staff to remove:");
        string username = Console.ReadLine();

        Console.Write("\nEnter the administrator password to confirm:");
        int adminPassword = int.Parse(Console.ReadLine());

        var admin = await _context.People.FindAsync(1);

        if (admin.Password != adminPassword && adminPassword != null)
        {
            var deletedStaff = await _context.Staffs
                .Where(s => s.Username == username)
                .FirstOrDefaultAsync();

            if (deletedStaff != null)
            {
                _context.Staffs.Remove(deletedStaff);
                await _context.SaveChangesAsync();
                Console.WriteLine("Staff removed successfully.");
            }
            else
            {
                Console.WriteLine("Staff with the given username not found.");
            }
        }
        else
            Console.WriteLine("Administrator password is incorrect. Staff removal aborted.");
    }
}
