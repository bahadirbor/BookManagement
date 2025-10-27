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

class Member : Person{
    public string Email { get; set; }

    public string? PhoneNumber { get; set; }
    
}

class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder){
        builder.Property(m => m.Email)
               .IsRequired()
               .HasMaxLength(40);
        
        builder.Property(m => m.PhoneNumber)
               .HasMaxLength(13);
    }
}

class MemberDto{
    //Created a Data Transfer Object for Member to facilitate data transfer
    [StringLength(20)]
    public string FirstName { get; set; }

    [StringLength(20)]
    public string Surname { get; set; }

    [StringLength(40)]
    public string Email { get; set; }

    [StringLength(13)]
    public string? PhoneNumber { get; set; }

    [MaxLength(16)]
    public int Password { get; set; }
}

class MemberOperations{
    //Staff operations such as Add, Update, Remove
    private readonly libraryDbContext _context;

    public MemberOperations() {
        _context = new libraryDbContext();
    }

    public async Task AddMember(MemberDto dto, Staff staff){

        string username = (dto.FirstName[0] + dto.Surname).ToLower();

        bool usernameExists = await _context.Members.AnyAsync(m => m.Username == username);

        Console.Write("Enter staff password to confirm adding member: ");
        int staffPassword = int.Parse(Console.ReadLine());

        if (usernameExists == false && staff.Password == staffPassword)
        {
            Member member = new Member()
            {
                FirstName = dto.FirstName,
                Surname = dto.Surname,
                Username = username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password
            };

            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }
        else{
            Console.WriteLine("Username already exists or Incorrect staff password.\nOperation aborted.");
            return;
        }
    }

    public async Task DeleteMember(Staff staff) {
        Console.Write("Enter the username of the member to delete: ");
        string username = Console.ReadLine();

        var member = await _context.Members.FirstOrDefaultAsync(m => m.Username == username);
        if (member != null) {
            Console.Write("Enter the staff password for confirmation: ");
            int password = int.Parse(Console.ReadLine());
            
            if (staff.Password == password) {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                Console.WriteLine("Member deleted successfully.");
            } 
            else {
                Console.WriteLine("Incorrect password. Deletion aborted.");
            }
        } 
        else {
            Console.WriteLine("Member not found.");
        }
    }

    public async Task UpdateMember(MemberDto dto, Staff staff) {
        Console.Write("Enter the username of the member to update: ");
        string username = Console.ReadLine();

        var member = await _context.Members.FirstOrDefaultAsync(m => m.Username == username);
       
        if (member != null) {
            Console.WriteLine("Current Member Details:");
            Console.WriteLine($"1. First Name: {member.FirstName}");
            Console.WriteLine($"2. Surname: {member.Surname}");
            Console.WriteLine($"3. Username: {member.Username}");
            Console.WriteLine($"4. Email: {member.Email}");
            Console.WriteLine($"5. Phone Number: {member.PhoneNumber}");
            
            Console.Write("\nEnter the number corresponding to the field you want to update: ");
            int choice = int.Parse(Console.ReadLine());

            Console.Write("\nEnter the staff password for confirmation: ");
            int password = int.Parse(Console.ReadLine());
            
            if (staff.Password == password) {
                switch (choice){
                    case 1:
                        member.FirstName = dto.FirstName;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member updated successfully.");
                        break;
                    case 2:
                        member.Surname = dto.Surname;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member updated successfully.");
                        break;
                    case 4:
                        member.Email = dto.Email;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member updated successfully.");
                        break;
                    case 5:
                        member.PhoneNumber = dto.PhoneNumber;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member updated successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Update aborted.");
                        return;
                }
            } 
            else {
                Console.WriteLine("Incorrect password. Update aborted.");
            }
        } 
        else {
            Console.WriteLine("Member not found.");
        }
    }
}