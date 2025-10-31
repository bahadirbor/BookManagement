﻿using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;


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
    [MaxLength(16)]
    public int Password { get; set; }
}

class StaffOperations{
    //Staff operations such as Add, Update, Remove
    private readonly LibraryDbContext _context;

    public StaffOperations() {
        _context = new LibraryDbContext();
    }

    public async Task AddStaffAsync(StaffDto dto){
        var admin = await _context.People.FindAsync(1);

        string username = (dto.FirstName[0] + dto.Surname).ToLower();

        bool usernameExists = await _context.Staffs
            .AnyAsync(s => s.Username == username);


        if(usernameExists == false && admin != null)
        {
            Console.Write("\nEnter the administrator password to confirm:");
            int adminPassword = int.Parse(Console.ReadLine());
            if(admin.Password != adminPassword){
                Console.WriteLine("Administrator password is incorrect. Staff addition aborted.");
                return;
            }
        }
        else{
            Console.WriteLine("Username already exists. Staff addition aborted.");
            return;
        }

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

    public async Task UpdateStaffAsync() {
        Console.Write("Enter the username of the staff to update:");
        string username = Console.ReadLine();

        if ("admin".Equals(username)){
            Console.WriteLine("Admin details cannot be updated");
            return;
        }

        var staff = await _context.Staffs
            .Where(s => s.Username == username)
            .FirstOrDefaultAsync();

        if (staff == null)
        {
            Console.WriteLine("Staff with the given username not found.");
            return;
        }
        else {
            Console.WriteLine("Current Staff Details:");
            Console.WriteLine($"1. First Name: {staff.FirstName}");
            Console.WriteLine($"2. Surname: {staff.Surname}");
            Console.WriteLine($"3. Username: {staff.Username}");
            Console.WriteLine("4. Password: ********");

            StaffDto dto = new StaffDto();

            Console.Write("\nEnter the number corresponding to the field you want to update: ");
            int choice = int.Parse(Console.ReadLine());

            Console.Write("\nEnter the admin password for confirmation: ");
            int password = int.Parse(Console.ReadLine());

            var admin = await _context.People.FindAsync(1);

            if (admin.Password == password)
            {
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter new First Name: ");
                        dto.FirstName = Console.ReadLine();

                        staff.FirstName = dto.FirstName;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member name updated successfully.");
                        break;
                    case 2:
                        Console.Write("Enter new Surname: ");
                        dto.Surname = Console.ReadLine();

                        staff.Surname = dto.Surname;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member surname updated successfully.");
                        break;
                    case 3:
                        Console.WriteLine("Username cannot be updated.");
                        break;
                    case 4:
                        Console.Write("Enter new Password (max 16 numbers): ");
                        dto.Password = int.Parse(Console.ReadLine());

                        staff.Password = dto.Password;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member password updated successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Update aborted.");
                        return;
                }
            }
            else
            {
                Console.WriteLine("Incorrect password. Update aborted.");
            }
        } 

    }

    public async Task RemoveStaffWithUsernameAysnc() {
        // Remove staff based on username
        Console.Write("Enter the username of the staff to remove:");
        string username = Console.ReadLine();

        if ("admin".Equals(username)){
            Console.WriteLine("Admin cannot be deleted");
            return;
        }

        Console.Write("\nEnter the administrator password to confirm:");
        int adminPassword = int.Parse(Console.ReadLine());

        var admin = await _context.People.FindAsync(1);

        if (admin.Password == adminPassword && adminPassword != null)
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
