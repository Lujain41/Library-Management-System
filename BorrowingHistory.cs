using System;
 namespace LibraryManagementSystem
{
public class BorrowingHistory
{
    public User User { get; }
    public DateTime BorrowedAt { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnedAt { get; private set; }

    public BorrowingHistory
    (User user, DateTime borrowedAt, TimeSpan loanDuration)
    {
        User = user;
        BorrowedAt = borrowedAt;
        DueDate = borrowedAt.Add(loanDuration);
        ReturnedAt = null;
    }

    public void MarkAsReturned(DateTime returnedAt)
    {
        ReturnedAt = returnedAt;
    }

    public bool IsOnTime()
    {
        return ReturnedAt <= DueDate;
    }
}}
