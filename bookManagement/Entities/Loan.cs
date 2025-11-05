using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookManagement.Entities;

class Loan{
    public int Id { get; set; }

    public int BookId { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime LoanDate { get; set; }
    
    public DateTime? EstimatedReturnDate { get; set; }

    public bool IsReturned { get; set; }

    public string WhoLoaned { get; set; }

    public string WhoReturned { get; set; }

    public string WhoModifiedLast { get; set; }
}

class LoanConfiguration : IEntityTypeConfiguration<Loan>{
    public void Configure(EntityTypeBuilder<Loan> builder){
        builder.HasKey(l => l.Id);

        builder.Property(l => l.LoanDate)
               .IsRequired();
    }
}

class LoanDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime LoanDate { get; set; }
    
    public DateTime? EstimatedReturnDate { get; set; }
    public bool IsReturned { get; set; }

    public string? WhoLoaned { get; set; }

    public string? WhoReturned { get; set; }

    public string WhoModifiedLast { get; set; }
}
class LoanOperations{
    // Add methods for loan operations here
    public readonly LibraryDbContext _context;

    public LoanOperations(){
        _context = new LibraryDbContext();
    }

    public async Task ShowLoansAsync(){
        var loans = await _context.Loans.ToListAsync();
        foreach(var loan in loans){
            Console.WriteLine($"Loan ID: {loan.Id}, Book ID: {loan.BookId}, User ID: {loan.UserId}, Loan Date: {loan.LoanDate}, Estimated Return Date: {loan.EstimatedReturnDate}, Is Returned: {loan.IsReturned}");
        }
    }

    public async Task ShowActiveLoansAsync(){
        var activeLoans = await _context.Loans
                                        .Where(l => !l.IsReturned)
                                        .ToListAsync();

        foreach(var loan in activeLoans){
            Console.WriteLine($"Loan ID: {loan.Id}, Book ID: {loan.BookId}, User ID: {loan.UserId}, Loan Date: {loan.LoanDate}, Estimated Return Date: {loan.EstimatedReturnDate}");
        }
    }

    public async Task ShowPastLoansAsync(){
        var passiveLoans = await _context.Loans
                                         .Where(l => l.IsReturned)
                                         .ToListAsync();
        foreach(var loan in passiveLoans){
            Console.WriteLine($"Loan ID: {loan.Id}, Book ID: {loan.BookId}, User ID: {loan.UserId}, Loan Date: {loan.LoanDate}, Estimated Return Date: {loan.EstimatedReturnDate}");
        }
    }

    public async Task ShowActiveLoansByUserAsync(Person person){
        var userActiveLoans = await _context.Loans
                                            .Where(l => l.UserId == person.PersonId && !l.IsReturned)
                                            .ToListAsync();
        foreach(var loan in userActiveLoans){
            Console.WriteLine($"Loan ID: {loan.Id}, Book ID: {loan.BookId}, Loan Date: {loan.LoanDate}, Estimated Return Date: {loan.EstimatedReturnDate}");
        }
    }

    public async Task ShowPastLoansByUserAsync(Person person){
        var userPastLoans = await _context.Loans
                                         .Where(l => l.UserId == person.PersonId && l.IsReturned)
                                         .ToListAsync();
        foreach(var loan in userPastLoans){
            Console.WriteLine($"Loan ID: {loan.Id}, Book ID: {loan.BookId}, Loan Date: {loan.LoanDate}, Returned Date: {loan.EstimatedReturnDate}");
        }
    }

    public async Task AddLoanAsync(LoanDto loanDto, Person person){
        var existingLoan = await _context.Books
                                      .FirstOrDefaultAsync(l => l.BookId == loanDto.BookId);

        if (existingLoan.IsLoaned == false)
        {
            var loan = new Loan
            {
                BookId = loanDto.BookId,
                UserId = loanDto.UserId,
                LoanDate = loanDto.LoanDate,
                EstimatedReturnDate = loanDto.LoanDate.Date.AddDays(14), // Assuming a 2-week loan period
                IsReturned = false,
                WhoLoaned = person.Username,
                WhoModifiedLast = person.Username
            };

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }
        else
            Console.WriteLine("The book is already loaned out.");
    }
    
    public async Task UpdateLoanAsync(Person person, Member member, int loanId){
        var loan = await _context.Loans.FindAsync(loanId);

        LoanDto updatedLoan = new LoanDto();

        if (loan != null){
            Console.WriteLine("Enter new Estimated Return Date (yyyy-MM-dd):");
            updatedLoan.EstimatedReturnDate = DateTime.Parse(Console.ReadLine());

            loan.EstimatedReturnDate = updatedLoan.EstimatedReturnDate;
            await _context.SaveChangesAsync();

            Console.WriteLine($"Loan return date updated {updatedLoan.EstimatedReturnDate} successfully.");
        }
        else
        {
            Console.WriteLine("No loan found with the provided ID.");
        }
    }

    public async Task ReturnLoanAsync(Person person, Member member){
        Console.WriteLine("Enter the Loaned Book Name to return:");
        string bookName = Console.ReadLine();

        var book = await _context.Books
                                 .FirstOrDefaultAsync(b => b.BookName == bookName && b.IsLoaned);
        
        var loan = await _context.Loans
                                 .FirstOrDefaultAsync(l => l.UserId == member.PersonId && !l.IsReturned && l.BookId == book.BookId);

        if (loan != null)
        {
            loan.IsReturned = true;
            loan.EstimatedReturnDate = DateTime.Now;
            loan.WhoReturned = person.Username;

            await _context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("No active loan found with the provided ID.");
        }
    }
}
