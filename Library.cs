using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem
{
    public class Library
    {
        // List to store books in the library
        private readonly List<Book> _books = new List<Book>();

        // List to store users of the library
        private readonly List<User> _users = new List<User>();

        // Notification service for sending notifications
        private readonly INotificationService<string> _notificationService;

        // Constructor that initializes the Library with a notification service
        public Library(INotificationService<string> notificationService)
        {
            // Ensure the notification service is not null
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        // Adds a book to the library and sends a success notification
        public void AddBook(Book book)
        {
            try
            {
                // Check if the book is null
                if (book == null)
                    throw new ArgumentNullException(nameof(book), "Book cannot be null.");
                 // Check if the book is already exists
                 if (_books.Any(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase)))
                {
                    _notificationService.SendNotificationOnDuplicate($"Book '{book.Title}' already exists.");
                    return;
                }

                // Add the book to the list of books
                _books.Add(book);

                // Notify success with the book title
                _notificationService.SendNotificationOnSuccess(book.Title);
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure(book.Title);
            }
        }

        // Adds a user to the library and sends a success notification
        public void AddUser(User user)
        {
            try
            {
                // Check if the user is null
                if (user == null)
                    throw new ArgumentNullException(nameof(user), "User cannot be null.");
                // Check if the user is already exists
                if (_users.Any(u => u.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    _notificationService.SendNotificationOnDuplicate($"User '{user.Name}' already exists.");
                    return;
                }

                // Add the user to the list of users
                _users.Add(user);

                // Notify success with the user name
                _notificationService.SendNotificationOnSuccess(user.Name);
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure(user.Name);
            }
        }

        // Deletes a book from the library by its ID and sends a success notification
        public void DeleteBook(Guid bookId)
        {
            try
            {
                // Find the book with the specified ID
                var bookToRemove = _books.FirstOrDefault(b => b.Id == bookId);
                if (bookToRemove == null)
                    throw new KeyNotFoundException("Book with the specified ID not found.");

                // Remove the book from the list
                _books.Remove(bookToRemove);

                // Notify success with the book ID
                _notificationService.SendNotificationOnSuccess($"Book with ID '{bookId}'");
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure($"Book with ID '{bookId}'");
            }
        }

        // Deletes a user from the library by their ID and sends a success notification
        public void DeleteUser(Guid userId)
        {
            try
            {
                // Find the user with the specified ID
                var userToRemove = _users.FirstOrDefault(u => u.Id == userId);
                if (userToRemove == null)
                    throw new KeyNotFoundException("User with the specified ID not found.");

                // Remove the user from the list
                _users.Remove(userToRemove);

                // Notify success with the user ID
                _notificationService.SendNotificationOnSuccess($"User with ID '{userId}'");
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure($"User with ID '{userId}'");
            }
        }

        // Retrieves a paginated list of books, sorted by their creation date
        public IEnumerable<Book> GetAllBooks(int pageNumber, int pageSize)
        {
            try
            {
                // Validate page number and size
                if (pageNumber < 1 || pageSize < 1)
                    throw new ArgumentException("Page number and page size must be greater than zero.");

                // Return the paginated list of books
                return _books
                    .OrderBy(b => b.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure("Retrieving books");
                return Enumerable.Empty<Book>();
            }
        }

        // Retrieves a paginated list of users, sorted by their creation date
        public IEnumerable<User> GetAllUsers(int pageNumber, int pageSize)
        {
            try
            {
                // Validate page number and size
                if (pageNumber < 1 || pageSize < 1)
                    throw new ArgumentException("Page number and page size must be greater than zero.");

                // Return the paginated list of users
                return _users
                    .OrderBy(u => u.CreatedDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure("Retrieving users");
                return Enumerable.Empty<User>();
            }
        }

        // Finds books whose titles contain the specified substring
        public IEnumerable<Book> FindBooksByTitle(string title)
        {
            try
            {
                // Validate the title
                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentException("Title cannot be null or empty.", nameof(title));

                // Return books that match the title search
                return _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure("Finding books");
                return Enumerable.Empty<Book>();
            }
        }

        // Finds users whose names contain the specified substring
        public IEnumerable<User> FindUsersByName(string name)
        {
            try
            {
                // Validate the name
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or empty.", nameof(name));

                // Return users that match the name search
                return _users.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                // Notify failure if an exception occurs
                _notificationService.SendNotificationOnFailure("Finding users");
                return Enumerable.Empty<User>();
            }
        }
       public void BorrowBook(Guid bookId, Guid userId, TimeSpan loanDuration, decimal penalty)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        var user = _users.FirstOrDefault(u => u.Id == userId);

        if (book == null)
        {
            _notificationService.SendNotificationOnFailure($"Book with ID {bookId} does not exist.");
            return;
        }

        if (user == null)
        {
            _notificationService.SendNotificationOnFailure($"User with ID {userId} does not exist.");
            return;
        }

        try
        {
            user.BorrowBook(book, loanDuration, penalty);
            Console.WriteLine($"User '{user.Title}' borrowed book '{book.Title}' on {DateTime.Now}. Due date is {DateTime.Now.Add(loanDuration)}.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void ReturnBook(Guid bookId, Guid userId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        var user = _users.FirstOrDefault(u => u.Id == userId);

        if (book == null)
        {
            _notificationService.SendNotificationOnFailure($"Book with ID {bookId} does not exist.");
            return;
        }

        if (user == null)
        {
            _notificationService.SendNotificationOnFailure($"User with ID {userId} does not exist.");
            return;
        }

        try
        {
            user.ReturnBook(book);
            var historyItem = book.BorrowingHistory.Last();
            if (historyItem.IsOnTime())
            {
                Console.WriteLine($"Book '{book.Title}' returned on time.");
            }
            else
            {
                Console.WriteLine($"Book '{book.Title}' returned late.");
            }
        }
        catch (InvalidOperationException ex)
        {
            _notificationService.SendNotificationOnFailure(ex.Message);
        }
    }

    public void CheckOverdueBooks()
    {
        Console.WriteLine("Checking overdue books...");
        foreach (var book in _books)
        {
            if (book.IsOverdue)
            {
                Console.WriteLine($"Book '{book.Title}' is overdue. Penalty: ${book.Penalty:F2}");
            }
        }
    }
}
}