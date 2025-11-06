using bookManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace bookManagement.Entities;

class Reservation{
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }

    //Navigation properties
    public Book Book { get; set; }
    public Member User { get; set; }
}

class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(p => p.ReservationId);

        builder.Property(p => p.ReservationDate)
               .IsRequired();
    }
}

class ReservationDto{
    public DateTime ReservationDate { get; set; }

    public string BookName { get; set; }
}

class ReservationOperations{
    private readonly LibraryDbContext _context;
    public ReservationOperations()
    {
        _context = new LibraryDbContext();
    }
    
    public async Task ShowMemberReservationsAsync(Person member){
        var reservations = await _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.User)
            .Where(r => r.UserId == member.PersonId)
            .ToListAsync();

        foreach (var reservation in reservations){
            Console.WriteLine($"Reservation ID: {reservation.ReservationId}, Date: {reservation.ReservationDate}, Book: {reservation.Book.BookName}, User: {reservation.User.FirstName}");
        }
    }

    public async Task ShowAllReservationsAsync(){
        var reservations = await _context.Reservations
            .Include(r => r.Book)
            .Include(r => r.User)
            .ToListAsync();
        foreach (var reservation in reservations){
            Console.WriteLine($"Reservation ID: {reservation.ReservationId}, Date: {reservation.ReservationDate}, Book: {reservation.Book.BookName}, User: {reservation.User.FirstName}");
        }
    }

    public async Task AddReservationAsync(ReservationDto reservationDto, Person member){
        var book = await _context.Books.FirstOrDefaultAsync(d => d.BookName == reservationDto.BookName);

        if (book == null){
            throw new Exception("Book not found");
        }
        
        var reservation = new Reservation
        {
            ReservationDate = DateTime.Now,
            BookId = book.BookId,
            UserId = member.PersonId
        };

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        
    }
    
    public async Task UpdateReservationAsync(int reservationId, ReservationDto reservationDto){
        var reservation = await _context.Reservations.FindAsync(reservationId);
        
        if (reservation == null){
            throw new Exception("Reservation not found");
        }
        else{
            var book = await _context.Books.FirstOrDefaultAsync(d => d.BookName == reservationDto.BookName);

            if (book == null){
                throw new Exception("Book not found");
            }

            reservation.ReservationDate = DateTime.Now;
            reservation.BookId = book.BookId;
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteReservationAsync(int reservationId){
        var reservation = await _context.Reservations.FindAsync(reservationId);
        
        if (reservation == null){
            throw new Exception("Reservation not found");
        }
        else{
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }

}
