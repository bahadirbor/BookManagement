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
    
    public string WhoModifiedLast { get; set; }
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
    private readonly LibraryDbContext _context;
    private readonly PersonOperations _personOperations;

    public MemberOperations() {
        _context = new LibraryDbContext();
        _personOperations = new PersonOperations();
    }

    public async Task AddMemberAsync(MemberDto dto, Person person){

        string username = (dto.FirstName[0] + dto.Surname).ToLower();

        bool usernameExists = await _context.Members.AnyAsync(m => m.Username == username);

        Console.Write("Enter staff password to confirm adding member: ");
        int Password = int.Parse(Console.ReadLine());

        if (usernameExists == false && _personOperations.Verify(person, Password))
        {
            Member member = new Member()
            {
                FirstName = dto.FirstName,
                Surname = dto.Surname,
                Username = username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Password = dto.Password,
                WhoModifiedLast = person.Username
            };

            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }
        else{
            Console.WriteLine("Username already exists or Incorrect staff password.\nOperation aborted.");
            return;
        }
    }

    public async Task DeleteMember(Person person) {
        Console.Write("Enter the username of the member to delete: ");
        string username = Console.ReadLine();

        var member = await _context.Members.FirstOrDefaultAsync(m => m.Username == username);

        if (member != null) {
            Console.Write("Enter the staff password for confirmation: ");
            int password = int.Parse(Console.ReadLine());
            
            if (_personOperations.Verify(person, password)) {
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

    public async Task UpdateMemberAsync(MemberDto dto, Person person) {
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
            Console.WriteLine($"6. Password");
            
            Console.Write("\nEnter the number corresponding to the field you want to update: ");
            int choice = int.Parse(Console.ReadLine());

            Console.Write("\nEnter the staff password for confirmation: ");
            int password = int.Parse(Console.ReadLine());
            
            if (_personOperations.Verify(person, password)) {
                switch (choice){
                    case 1:
                        member.FirstName = dto.FirstName;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member name updated successfully.");
                        break;
                    case 2:
                        member.Surname = dto.Surname;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member surname updated successfully.");
                        break;
                    case 3:
                        Console.WriteLine("Username cannot be changed. Update aborted.");
                        return;
                    case 4:
                        member.Email = dto.Email;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member email updated successfully.");
                        break;
                    case 5:
                        member.PhoneNumber = dto.PhoneNumber;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member phone number updated successfully.");
                        break;
                    case 6:
                        member.Password = dto.Password;
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Member password updated successfully.");
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

    public async Task ShowAllMembersAsync(){
        
        var members = await _context.Members.ToListAsync();

        foreach(var member in members){
            Console.WriteLine($"\nMember ID: {member.PersonId}");
            Console.WriteLine($"First Name: {member.FirstName}");
            Console.WriteLine($"Surname: {member.Surname}");
            Console.WriteLine($"Username: {member.Username}");
            Console.WriteLine($"Email: {member.Email}");
            Console.WriteLine($"Phone Number: {member.PhoneNumber}");
            Console.WriteLine(new string('-', 40));
        }
    }
}