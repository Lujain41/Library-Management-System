using System;
using System.Linq;

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main()
        {
            // Initialize notification services
            var emailService = new EmailNotificationService();
            var smsService = new SMSNotificationService();

            // Create libraries with different notification services
            var libraryWithEmail = new Library(emailService);
            var libraryWithSMS = new Library(smsService);

            // Define some users and books
            var users = new[]
            {
                new User("Alice", new DateTime(2023, 1, 1)),
                new User("Bob", new DateTime(2023, 2, 1)),
                new User("Charlie", new DateTime(2023, 3, 1)),
                new User("David", new DateTime(2023, 4, 1)),
                new User("Eve", new DateTime(2024, 5, 1)),
                new User("Fiona", new DateTime(2024, 6, 1)),
                new User("George", new DateTime(2024, 7, 1)),
                new User("Hannah", new DateTime(2024, 8, 1)),
                new User("Ian"),
                new User("Julia")
            };

            var books = new[]
            {
                new Book("The Great Gatsby", new DateTime(2023, 1, 1)),
                new Book("1984", new DateTime(2023, 2, 1)),
                new Book("To Kill a Mockingbird", new DateTime(2023, 3, 1)),
                new Book("The Catcher in the Rye", new DateTime(2023, 4, 1)),
                new Book("Pride and Prejudice", new DateTime(2023, 5, 1)),
                new Book("Wuthering Heights", new DateTime(2023, 6, 1)),
                new Book("Jane Eyre", new DateTime(2023, 7, 1)),
                new Book("Brave New World", new DateTime(2023, 8, 1)),
                new Book("Moby-Dick", new DateTime(2023, 9, 1)),
                new Book("War and Peace", new DateTime(2023, 10, 1)),
                new Book("Hamlet", new DateTime(2023, 11, 1)),
                new Book("Great Expectations", new DateTime(2023, 12, 1)),
                new Book("Ulysses", new DateTime(2024, 1, 1)),
                new Book("The Odyssey", new DateTime(2024, 2, 1)),
                new Book("The Divine Comedy", new DateTime(2024, 3, 1)),
                new Book("Crime and Punishment", new DateTime(2024, 4, 1)),
                new Book("The Brothers Karamazov", new DateTime(2024, 5, 1)),
                new Book("Don Quixote", new DateTime(2024, 6, 1)),
                new Book("The Iliad"),
                new Book("Anna Karenina"),
                new Book("The Great Gatsby", new DateTime(2023, 1, 1))//Duplicate entry
            };

            // Add and remove books and users in both libraries
            ManageLibrary(libraryWithEmail, users, books);
            ManageLibrary(libraryWithSMS, users, books);

            // Demonstrate borrowing and returning with penalties
            Console.WriteLine("=== Borrowing Books ===");
            libraryWithEmail.BorrowBook(books[0].Id, users[0].Id, TimeSpan.FromDays(7), 1.00m);
            libraryWithSMS.BorrowBook(books[1].Id, users[1].Id, TimeSpan.FromDays(10), 0.50m);
            libraryWithEmail.BorrowBook(books[0].Id, users[1].Id, TimeSpan.FromDays(7), 1.00m); // should show that the book is already borrowed

            // Simulate some time passing
            System.Threading.Thread.Sleep(5000); // Sleep for 5 seconds to simulate delay

            Console.WriteLine("\n=== Returning Books ===");
            libraryWithEmail.ReturnBook(books[0].Id, users[0].Id);
            libraryWithSMS.ReturnBook(books[1].Id, users[1].Id);

            Console.WriteLine("\n=== Checking Overdue Books ===");
            libraryWithEmail.CheckOverdueBooks();
        }

        private static void ManageLibrary(Library library, User[] users, Book[] books)
        {
            Console.WriteLine($"Managing library with {library.GetType().Name}");

            // Add users to the library
            foreach (var user in users)
            {
                library.AddUser(user);
            }

            // Add books to the library
            foreach (var book in books)
            {
                library.AddBook(book);
            }

            // Display paginated list of books
            DisplayBooks(library, 1, 5);

            // Display paginated list of users
            DisplayUsers(library, 1, 5);

            // Find books by title
            var searchTitle = "1984";
            var foundBooks = library.FindBooksByTitle(searchTitle);
            Console.WriteLine($"\nBooks found with title containing '{searchTitle}':");
            foreach (var book in foundBooks)
            {
                Console.WriteLine($"{book.Id}: {book.Title} (Created: {book.CreatedDate})");
            }

            // Find users by name
            var searchName = "Alice";
            var foundUsers = library.FindUsersByName(searchName);
            Console.WriteLine($"\nUsers found with name containing '{searchName}':");
            foreach (var user in foundUsers)
            {
                Console.WriteLine($"{user.Id}: {user.Name} (Created: {user.CreatedDate})");
            }

            // Example: Delete a book by ID
            var bookToDelete = library.GetAllBooks(1, 5).FirstOrDefault();
            if (bookToDelete != null)
            {
                library.DeleteBook(bookToDelete.Id);
                Console.WriteLine($"\nDeleted Book with ID: {bookToDelete.Id}");
            }
            else
            {
                Console.WriteLine("\nNo book found to delete.");
            }

            // Example: Delete a user by ID
            var userToDelete = library.GetAllUsers(1, 5).FirstOrDefault();
            if (userToDelete != null)
            {
                library.DeleteUser(userToDelete.Id);
                Console.WriteLine($"\nDeleted User with ID: {userToDelete.Id}");
            }
            else
            {
                Console.WriteLine("\nNo user found to delete.");
            }

            Console.WriteLine(); // Add an empty line for separation
        }

        private static void DisplayBooks(Library library, int pageNumber, int pageSize)
        {
            Console.WriteLine($"\nAll Books (Page {pageNumber}, PageSize {pageSize}):");
            foreach (var book in library.GetAllBooks(pageNumber, pageSize))
            {
                Console.WriteLine($"{book.Id}: {book.Title} (Created: {book.CreatedDate})");
            }
        }

        private static void DisplayUsers(Library library, int pageNumber, int pageSize)
        {
            Console.WriteLine($"\nAll Users (Page {pageNumber}, PageSize {pageSize}):");
            foreach (var user in library.GetAllUsers(pageNumber, pageSize))
            {
                Console.WriteLine($"{user.Id}: {user.Name} (Created: {user.CreatedDate})");
            }
        }
    }
}
