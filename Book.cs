using System;

namespace LibraryManagementSystem
{
    public class Book : BaseEntity
    {
        // Title of the book
        public string Title { get; set; }
        public User BorrowedBy {get; set; }
        public User CurrentBorrower { get; private set; }
        public DateTime? BorrowDate { get; private set; }
        public DateTime? DueDate { get; private set; }
        public bool IsOverdue => DueDate.HasValue && DateTime.Now > DueDate.Value;
        public decimal Penalty { get; private set; } // Penalty for overdue books
    

        public List<BorrowingHistory> BorrowingHistory { get; } = new List<BorrowingHistory>();



        // Constructor to initialize Book with title and optional creation date
        public Book(string title, DateTime? createdDate = null) : base( title, createdDate)
        {
            // Set title
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        // Override ToString method for better readability
        public override string ToString()
        {
            return $"Book: {Title} (Created: {CreatedDate})";
        }
        public bool IsBorrowed => BorrowedBy != null;

       public void Borrow(User user, TimeSpan loanDuration, decimal penalty)
    {
        if (IsBorrowed)
            throw new InvalidOperationException("The book is already borrowed.");

        CurrentBorrower = user;
        BorrowDate = DateTime.Now;
        DueDate = BorrowDate.Value.Add(loanDuration);
        Penalty = penalty;
        BorrowingHistory.Add(new BorrowingHistory(user, BorrowDate.Value, loanDuration));
    }

    public decimal Return()
    {
        if (!IsBorrowed)
            throw new InvalidOperationException("The book was not borrowed.");

        var history = BorrowingHistory.Last();
        history.MarkAsReturned(DateTime.Now);
        CurrentBorrower = null;

        // Calculate penalty if overdue
        decimal penaltyAmount = 0;
        if (IsOverdue)
        {
            TimeSpan overdueDuration = DateTime.Now - DueDate.Value;
            penaltyAmount = (decimal)overdueDuration.TotalDays * Penalty;
        }

        BorrowDate = null;
        DueDate = null;

        return penaltyAmount;
    }
}
}