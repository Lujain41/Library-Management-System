using System;

namespace LibraryManagementSystem
{
    public class User : BaseEntity
    {
        // Name of the user
        public string Name { get; set; }
        public List<BorrowingHistory> BorrowingHistory { get; } = new List<BorrowingHistory>();
        public decimal Points { get; private set; } // Points for handling fines or penalties


        // Constructor to initialize User with name and optional creation date
        public User(string name, DateTime? createdDate = null) : base(name, createdDate)
        {
            // Set name
            Name = name ?? throw new ArgumentNullException(nameof(name));
            
        }

        // Override ToString method for better readability
        public override string ToString()
        {
            return $"User: {Name} (Created: {CreatedDate})";
        }

    public void BorrowBook(Book book, TimeSpan loanDuration, decimal penalty)
    {
        if (book.IsBorrowed)
            throw new InvalidOperationException("The book is already borrowed.");

        book.Borrow(this, loanDuration, penalty);
        BorrowingHistory.Add(new BorrowingHistory(this, DateTime.Now, loanDuration));
    }

    public void ReturnBook(Book book)
    {
        if (!book.IsBorrowed || book.CurrentBorrower.Id != Id)
            throw new InvalidOperationException("The book was not borrowed by this user.");

        decimal penalty = book.Return();
        if (penalty > 0)
        {
            Points -= penalty; // Deduct points or add penalty
            Console.WriteLine($"Book '{book.Title}' returned late. Penalty applied: ${penalty:F2}");
        }
        else
        {
            Console.WriteLine($"Book '{book.Title}' returned on time.");
        }

        var historyItem = BorrowingHistory.Last();
        historyItem.MarkAsReturned(DateTime.Now);
    }

    public void CheckOverdueBooks()
    {
        var overdueBooks = BorrowingHistory
            .Where(item => item.ReturnedAt == null && item.DueDate < DateTime.Now)
            .ToList();

        if (overdueBooks.Any())
        {
            Console.WriteLine("Overdue Books:");
            foreach (var item in overdueBooks)
            {
                Console.WriteLine($"Book borrowed on {item.BorrowedAt} is overdue.");
            }
        }
        else
        {
            Console.WriteLine("No overdue books.");
        }
    }
}
}