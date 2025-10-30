using Microsoft.EntityFrameworkCore;
using bookManagement.Data;
using bookManagement.Entities;

PersonOperations personOperations = new PersonOperations();

while (true)
{
    Console.WriteLine("Welcome");
    Console.Write("Enter your username: ");
    string username = Console.ReadLine();

    Console.Write("Enter your password: ");
    int password = int.Parse(Console.ReadLine());

    var user = await personOperations.LoginAsync(username, password);

    if (user == null)
        continue;

    switch (user.PersonType)
    {
        case "Person":
            Console.WriteLine($"Welcome boss!");
            Console.WriteLine("1. Staff Operations");
            Console.WriteLine("2. Member Operations");
            Console.WriteLine("3. Book Operations");
            Console.WriteLine("4. Loan Operations");
            Console.WriteLine("5. Show All Books");
            Console.WriteLine("6. Exit");

            Console.Write("Enter your choice: ");
            int decision = int.Parse(Console.ReadLine());
            if (decision == 6)
                Environment.Exit(0);

            break;
        case "Staff":
            Console.WriteLine($"Welcome Staff Member: {user.FirstName} {user.Surname}");
            Console.WriteLine("1. Member Operations");
            Console.WriteLine("2. Book Operations");
            Console.WriteLine("3. Loan Operations");
            Console.WriteLine("4. Show All Books");
            break;
        case "Member":
            Console.WriteLine($"Welcome Member: {user.FirstName} {user.Surname}");
            Console.WriteLine("1. Reservation Operations");
            Console.WriteLine("2. Show All Books");

            break;
        default:
            Console.WriteLine("Unknown user type.");
            break;
    }
}
