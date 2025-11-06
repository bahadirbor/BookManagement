using bookManagement.Data;
using bookManagement.Entities;
using Microsoft.EntityFrameworkCore;

PersonOperations personOperations = new PersonOperations();
LibraryDbContext context = new LibraryDbContext();
bool exit = false;

while (true)
{
    exit = false; //Reset exit flag for each login attempt

    Console.WriteLine("Welcome");
    Console.Write("Enter your username: ");
    string username = Console.ReadLine();

    Console.Write("Enter your password: ");
    int password = int.Parse(Console.ReadLine());

    var user = await personOperations.LoginAsync(username, password);

    if (user == null)
        continue;

    while (!exit){
        //If you choose exit, you will logout and return to the login screen
        switch (user.PersonType)
        {
            #region Admin Menu
            case "Person":
                Console.WriteLine("\nWelcome boss!");
                Console.WriteLine("1. Staff Operations");
                Console.WriteLine("2. Member Operations");
                Console.WriteLine("3. Book Operations");
                Console.WriteLine("4. Loan Operations");
                Console.WriteLine("5. Show All Books");
                Console.WriteLine("6. Show All Members");
                Console.WriteLine("7. Show All Staff");
                Console.WriteLine("8. Exit");

                Console.Write("Enter the operation decision number: ");
                int adminDecision = int.Parse(Console.ReadLine());

                switch (adminDecision)
                {
                    case 1:
                        // Staff Operations
                        StaffOperations staffOperations = new StaffOperations();
                        Console.WriteLine("1. Add Staff");
                        Console.WriteLine("2. Update Staff");
                        Console.WriteLine("3. Remove Staff");
                        Console.WriteLine("4. Return the main menu");

                        Console.Write("Enter the staff operation number: ");
                        int adminStaffOperationDecision = int.Parse(Console.ReadLine());

                        switch (adminStaffOperationDecision)
                        {
                            case 1:
                                // Add Staff
                                StaffDto newStaff = new StaffDto();
                                string[] positions = { "Library Manager", "Librarian", "Assistant Librarian", "Archivist", "Library Technician"};

                                Console.Write("Enter First Name: ");
                                newStaff.FirstName = Console.ReadLine();
                                
                                Console.Write("Enter Surname: ");
                                newStaff.Surname = Console.ReadLine();
                                
                                Console.Write("Enter Password (max 16 numbers): ");
                                newStaff.Password = int.Parse(Console.ReadLine());

                                int num = 1;
                                foreach(string pos in positions)
                                {
                                    Console.WriteLine($"{num} {pos}");
                                    num++;
                                }
                                Console.Write("Select a position with number: ");
                                num = int.Parse(Console.ReadLine());
                                newStaff.Position = positions[num - 1];

                                await staffOperations.AddStaffAsync(newStaff, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                // Update Staff
                                staffOperations.UpdateStaffAsync(user);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                // Remove Staff
                                staffOperations.RemoveStaffWithUsernameAysnc(user);
                                await Task.Delay(1000);
                                break;
                            case 4:
                                // Return to main menu
                                Console.WriteLine("\nReturning to main menu...");
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        // Admin's Member Operations
                        MemberOperations memberOperations = new MemberOperations();
                        Console.WriteLine("\n1. Add Member");
                        Console.WriteLine("2. Update Member");
                        Console.WriteLine("3. Remove Member");
                        Console.WriteLine("4. Return the main menu\n");

                        Console.Write("Enter the member operation number: ");
                        int adminMemberOperationDecision = int.Parse(Console.ReadLine());

                        switch (adminMemberOperationDecision){
                            case 1:
                                // Add Member
                                MemberDto memberAddDto = new MemberDto();

                                Console.Write("Enter First Name: ");
                                memberAddDto.FirstName = Console.ReadLine();
                                
                                Console.Write("Enter Surname: ");
                                memberAddDto.Surname = Console.ReadLine();
                                
                                Console.Write("Enter Email : ");
                                memberAddDto.Email = Console.ReadLine();
                                
                                Console.Write("Enter Phone Number (optional): ");
                                memberAddDto.PhoneNumber = Console.ReadLine();
                                
                                Console.Write("Enter Password (max 16 numbers): ");
                                memberAddDto.Password = int.Parse(Console.ReadLine());

                                await memberOperations.AddMemberAsync(memberAddDto, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                // Update Member
                                MemberDto memberUpdateDto = new MemberDto();
                                await memberOperations.UpdateMemberAsync(memberUpdateDto, user);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                // Remove Member
                                await memberOperations.RemoveMemberWithUsernameAsync(user);
                                await Task.Delay(1000);
                                break;    
                            case 4:
                                // Return to main menu
                                Console.WriteLine("\nReturning to main menu...");
                                await Task.Delay(1000);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 3:
                        // Admin's Book Operations
                        BookOperations bookOperations = new BookOperations();
                        Console.WriteLine("1. Add Book");
                        Console.WriteLine("2. Update Book");
                        Console.WriteLine("3. Remove Book");
                        Console.WriteLine("4. Return to main menu");

                        Console.WriteLine("Enter the book operation number: ");
                        int adminBookOperationDecision = int.Parse(Console.ReadLine());

                        switch (adminBookOperationDecision){
                            case 1:
                                // Add Book
                                BookDto newBook = new BookDto();

                                Console.Write("Enter Book Name: ");
                                newBook.BookName = Console.ReadLine();

                                Console.Write("Enter ISBN: ");
                                newBook.ISBN = Console.ReadLine();

                                Console.Write("Enter Publication Date (yyyy-mm-dd): ");
                                newBook.PublicationDate = Console.ReadLine();

                                Console.Write("Enter Publisher Name: ");
                                newBook.PublisherName = Console.ReadLine();

                                List<string> Authors = new List<string>();

                                while (true){
                                    Console.Write("Enter Author Name and Surname or press 'x' to stop:");
                                    string nameSurname = Console.ReadLine();
                                    if (!"x".Equals(nameSurname.ToLower()) && nameSurname != null)
                                        Authors.Add(nameSurname);
                                    else
                                        break;
                                }
                                newBook.AuthorNameSurname = Authors;

                                List<string> Categories = new List<string>();

                                while (true){
                                    Console.Write("Enter another Category Name or press 'x' to stop:");
                                    string categoryName = Console.ReadLine();
                                    if (!"x".Equals(categoryName.ToLower()) && categoryName != null)
                                        Categories.Add(categoryName);
                                    else
                                        break;
                                }
                                newBook.Categories = Categories;

                                await bookOperations.AddBookAsync(newBook, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                BookDto updateBook = new BookDto();
                                await bookOperations.UpdateBookAsync(updateBook, user);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                await bookOperations.RemoveBookWithNameAsync();
                                await Task.Delay(1000);
                                break;
                            case 4:
                                Console.WriteLine("\nReturning to main menu...");
                                await Task.Delay(1000);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 4:
                        // Admin's Loan Operations
                        LoanOperations loanOperations = new LoanOperations();
                        Console.WriteLine("1. Add Loan");
                        Console.WriteLine("2. Update Loan");
                        Console.WriteLine("3. Return Loan");
                        Console.WriteLine("4. Show All Active Loans");
                        Console.WriteLine("5. Return to main menu");
                        
                        Console.Write("Enter the loan operation number: ");
                        int adminLoanOperationDecision = int.Parse(Console.ReadLine());

                        switch (adminLoanOperationDecision){
                            case 1:
                                // Add Loan
                                LoanDto newLoan = new LoanDto();
                                Console.Write("Enter Book ID: ");
                                newLoan.BookId = int.Parse(Console.ReadLine());
                                Console.Write("Enter Member ID: ");
                                newLoan.UserId = int.Parse(Console.ReadLine());
                                newLoan.LoanDate = DateTime.Now;
                                await loanOperations.AddLoanAsync(newLoan, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                // Update Loan
                                await loanOperations.ShowActiveLoansAsync();
                                Console.Write("Enter Loan ID for updating loan: ");
                                int loanIdToUpdate = int.Parse(Console.ReadLine());

                                await loanOperations.UpdateLoanAsync(user, loanIdToUpdate);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                //Return Loan
                                Console.WriteLine("Enter the member name: ");
                                string memberName = Console.ReadLine();
                                string[] parseName = memberName.Split(' ');

                                Member member = await context.Members.FirstOrDefaultAsync(m => m.FirstName == parseName[0] && m.Surname == parseName[1]);
                                if (member != null)
                                    await loanOperations.ReturnLoanAsync(user, member);
                                else
                                    Console.WriteLine("User not found");
                                break;
                            case 4:
                                //Show all loans
                                await loanOperations.ShowActiveLoansAsync();
                                await Task.Delay(1000);
                                break;
                            case 5:
                                Console.WriteLine("\nReturning to main menu...");
                                await Task.Delay(1000);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 5:
                        // Show All Books
                        BookOperations showBooks = new BookOperations();
                        await showBooks.ShowAllBooksAsync();
                        await Task.Delay(1000);
                        break;    
                    case 6:
                        // Show All Members
                        MemberOperations showMembers = new MemberOperations();

                        await showMembers.ShowAllMembersAsync();
                        await Task.Delay(1000);
                        break;
                    case 7:
                        // Show All Staff
                        StaffOperations showStaffs = new StaffOperations();

                        await showStaffs.ShowAllStaffsAsync();
                        await Task.Delay(1000);
                        break;
                    case 8:
                        Console.WriteLine("Exiting...");
                        exit = true;

                        await Task.Delay(1000);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                break;
            #endregion

            #region Staff Menu
            case "Staff":
                Console.WriteLine($"\nWelcome Staff Member: {user.FirstName} {user.Surname}");
                Console.WriteLine("1. Book Operations");
                Console.WriteLine("2. Loan Operations");
                Console.WriteLine("3. Member Operations");
                Console.WriteLine("4. Show All Members");
                Console.WriteLine("5. Show All Books");
                Console.WriteLine("6. Show All Loans");
                Console.WriteLine("7. Exit");

                Console.Write("Enter the operation decision number: ");
                int staffDecisionNumber = int.Parse(Console.ReadLine());

                switch (staffDecisionNumber)
                {
                    case 1:
                        // Book Operations
                        BookOperations staffBookOperations = new BookOperations();

                        Console.WriteLine("1. Add Book");
                        Console.WriteLine("2. Update Book");
                        Console.WriteLine("3. Remove Book");
                        Console.WriteLine("4. Return to main menu");

                        Console.WriteLine("Enter the book operation number: ");
                        int staffBookOperationDecision = int.Parse(Console.ReadLine());

                        switch (staffBookOperationDecision)
                        {
                            case 1:
                                // Add Book
                                BookDto newBook = new BookDto();

                                Console.Write("Enter Book Name: ");
                                newBook.BookName = Console.ReadLine();

                                Console.Write("Enter ISBN: ");
                                newBook.ISBN = Console.ReadLine();

                                Console.Write("Enter Publication Date (yyyy-mm-dd): ");
                                newBook.PublicationDate = Console.ReadLine();

                                Console.Write("Enter Publisher Name: ");
                                newBook.PublisherName = Console.ReadLine();

                                List<string> Authors = new List<string>();

                                while (true)
                                {
                                    Console.Write("Enter Author Name and Surname or press 'x' to stop:");
                                    string nameSurname = Console.ReadLine();
                                    if (!"x".Equals(nameSurname.ToLower()) && nameSurname != null)
                                        Authors.Add(nameSurname);
                                    else
                                        break;
                                }
                                newBook.AuthorNameSurname = Authors;

                                List<string> Categories = new List<string>();

                                while (true)
                                {
                                    Console.Write("Enter another Category Name or press 'x' to stop:");
                                    string categoryName = Console.ReadLine();
                                    if (!"x".Equals(categoryName.ToLower()) && categoryName != null)
                                        Categories.Add(categoryName);
                                    else
                                        break;
                                }
                                newBook.Categories = Categories;

                                await staffBookOperations.AddBookAsync(newBook, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                BookDto updateBook = new BookDto();
                                await staffBookOperations.UpdateBookAsync(updateBook, user);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                await staffBookOperations.RemoveBookWithNameAsync();
                                await Task.Delay(1000);
                                break;
                            case 4:
                                Console.WriteLine("\nReturning to main menu...");
                                await Task.Delay(1000);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        // Loan Operations
                        LoanOperations loanOperations = new LoanOperations();
                        Console.WriteLine("1. Add Loan");
                        Console.WriteLine("2. Update Loan");
                        Console.WriteLine("3. Return Loan");
                        Console.WriteLine("4. Return to main menu");

                        Console.Write("Enter the loan operation number: ");
                        int staffLoanOperationDecision = int.Parse(Console.ReadLine());

                        switch (staffLoanOperationDecision)
                        {
                            case 1:
                                // Add Loan
                                LoanDto newLoan = new LoanDto();
                                Console.Write("Enter Book ID: ");
                                newLoan.BookId = int.Parse(Console.ReadLine());
                                Console.Write("Enter Member ID: ");
                                newLoan.UserId = int.Parse(Console.ReadLine());
                                newLoan.LoanDate = DateTime.Now;
                                await loanOperations.AddLoanAsync(newLoan, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                // Update Loan
                                await loanOperations.ShowActiveLoansAsync();
                                Console.Write("Enter Loan ID for updating loan: ");
                                int loanIdToUpdate = int.Parse(Console.ReadLine());

                                await loanOperations.UpdateLoanAsync(user, loanIdToUpdate);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                //Return Loan
                                Console.WriteLine("Enter the member name: ");
                                string memberName = Console.ReadLine();
                                string[] parseName = memberName.Split(' ');

                                Member member = await context.Members.FirstOrDefaultAsync(m => m.FirstName == parseName[0] && m.Surname == parseName[1]);
                                if (member != null)
                                    await loanOperations.ReturnLoanAsync(user, member);
                                else
                                    Console.WriteLine("User not found");
                                break;
                            case 4:
                                Console.WriteLine("\nReturning to main menu...");
                                await Task.Delay(1000);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 3:
                        // Member Operations
                        MemberOperations memberOperations = new MemberOperations();

                        Console.WriteLine("1. Add Member");
                        Console.WriteLine("2. Update Member");
                        Console.WriteLine("3. Remove Member");
                        Console.WriteLine("4. Return to main menu");

                        Console.Write("Enter the member operation number: ");
                        int memberOperationDecision = int.Parse(Console.ReadLine());

                        switch (memberOperationDecision)
                        {
                            case 1:
                                // Add Member
                                MemberDto memberAddDto = new MemberDto();

                                Console.Write("Enter First Name: ");
                                memberAddDto.FirstName = Console.ReadLine();

                                Console.Write("Enter Surname: ");
                                memberAddDto.Surname = Console.ReadLine();

                                Console.Write("Enter Email : ");
                                memberAddDto.Email = Console.ReadLine();

                                Console.Write("Enter Phone Number (optional): ");
                                memberAddDto.PhoneNumber = Console.ReadLine();

                                Console.Write("Enter Password (max 16 numbers): ");
                                memberAddDto.Password = int.Parse(Console.ReadLine());

                                await memberOperations.AddMemberAsync(memberAddDto, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                // Update Member
                                MemberDto memberUpdateDto = new MemberDto();
                                await memberOperations.UpdateMemberAsync(memberUpdateDto, user);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                // Remove Member
                                await memberOperations.RemoveMemberWithUsernameAsync(user);
                                await Task.Delay(1000);
                                break;
                            case 4:
                                // Return to main menu
                                Console.WriteLine("\nReturning to main menu...");
                                await Task.Delay(1000);
                                break;
                        }
                        break;
                    case 4:
                        // Show All Members
                        MemberOperations showMembers = new MemberOperations();

                        await showMembers.ShowAllMembersAsync();
                        await Task.Delay(1000);
                        break;
                    case 5:
                        // Show All Books
                        BookOperations showBooks = new BookOperations();

                        await showBooks.ShowAllBooksAsync();
                        await Task.Delay(1000);
                        break;
                    case 6:
                        // Show All Loans
                        LoanOperations showLoans = new LoanOperations();
                        await showLoans.ShowActiveLoansAsync();
                        await Task.Delay(1000);
                        break;
                    case 7:
                        Console.WriteLine("Exiting...");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                break;
            #endregion

            #region Member Menu
            case "Member":
                Console.WriteLine($"Welcome Member: {user.FirstName} {user.Surname}");
                Console.WriteLine("1. Reservation Operations");
                Console.WriteLine("2. Show My Reservations");
                Console.WriteLine("3. Show All Books");
                Console.WriteLine("4. Show My Active Loans");
                Console.WriteLine("5. Show My Past Loans");
                Console.WriteLine("6. Exit");

                Console.Write("Enter the operation decision number: ");
                int memberDecisionNumber = int.Parse(Console.ReadLine());

                switch( memberDecisionNumber)
                {
                    case 1:
                        // Reservation Operations
                        ReservationOperations reservationOperations = new ReservationOperations();
                        Console.WriteLine("1. Add Reservation");
                        Console.WriteLine("2. Change Reservation Informations");
                        Console.WriteLine("3. Delete Reservation");

                        Console.Write("Enter the operation number: ");
                        int memberReservationDecisionNumber = int.Parse(Console.ReadLine());

                        ReservationDto reservationDto = new ReservationDto();

                        switch (memberReservationDecisionNumber){
                            case 1:
                                Console.Write("Enter the book name: ");
                                reservationDto.BookName = Console.ReadLine();

                                await reservationOperations.AddReservationAsync(reservationDto, user);
                                await Task.Delay(1000);
                                break;
                            case 2:
                                await reservationOperations.ShowMemberReservationsAsync(user);
                                await Task.Delay(1000);

                                Console.Write("Enter the reservation id to change: ");
                                int reservationUpdateId = int.Parse(Console.ReadLine());

                                var reservation = await context.Reservations.FindAsync(reservationUpdateId);
                                Console.WriteLine($"Reservation ID: {reservation.ReservationId}, Date: {reservation.ReservationDate}, Book: {reservation.Book.BookName}, User: {reservation.User.FirstName}");

                                Console.WriteLine("\nEnter the new book name: ");
                                reservationDto.BookName = Console.ReadLine();

                                await reservationOperations.UpdateReservationAsync(reservationUpdateId, reservationDto);
                                await Task.Delay(1000);
                                break;
                            case 3:
                                await reservationOperations.ShowMemberReservationsAsync(user);
                                await Task.Delay(1000);

                                Console.Write("Enter the reservation id to delete: ");
                                int reservationDeleteId = int.Parse(Console.ReadLine());

                                await reservationOperations.DeleteReservationAsync(reservationDeleteId);
                                await Task.Delay(1000);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        //Show member's reservations
                        ReservationOperations showMemberReservations = new ReservationOperations();
                        await showMemberReservations.ShowMemberReservationsAsync(user);

                        break;
                    case 3:
                        // Show All Books
                        BookOperations showBooks = new BookOperations();

                        await showBooks.ShowAllBooksAsync();
                        await Task.Delay(1000);
                        break;
                    case 4:
                        // Show My Active Loans
                        LoanOperations loanOperations = new LoanOperations();

                        await loanOperations.ShowActiveLoansByUserAsync(user);
                        await Task.Delay(1000);
                        break;
                    case 5:
                        // Show My Past Loans
                        LoanOperations loanOps = new LoanOperations();

                        await loanOps.ShowPastLoansByUserAsync(user);
                        await Task.Delay(1000);
                        break;
                    case 6:
                        Console.WriteLine("Exiting...");
                        exit = true;
                        await Task.Delay(1000);
                        break;
                    default:
                        break;
                }
                break;
            #endregion
            default:
                Console.WriteLine("Invalid user type.");
                break;

        }
    }
}
