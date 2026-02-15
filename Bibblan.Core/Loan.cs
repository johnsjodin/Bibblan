namespace Bibblan.Core;

public class Loan
{
    public Book Book { get; private set; }
    public Member Member { get; private set; }
    public DateTime LoanDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public Loan(Book book, Member member, DateTime loanDate, DateTime dueDate)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book), "Boken får inte vara null.");
        if (member == null)
            throw new ArgumentNullException(nameof(member), "Medlemmen får inte vara null.");
        if (loanDate > dueDate)
            throw new ArgumentException("Lånedatum måste vara före förfallodatum.");
        Book = book;
        Member = member;
        LoanDate = loanDate;
        DueDate = dueDate;
        ReturnDate = null;
    }

    public bool IsOverdue => ReturnDate == null && DateTime.Now > DueDate;

    public bool IsReturned => ReturnDate != null;

    public decimal CalculateLateFee(decimal dailyFee, DateTime? asOf = null)
    {
        if (dailyFee < 0)
            throw new ArgumentOutOfRangeException(nameof(dailyFee), "Avgiften kan inte vara negativ.");

        var referenceDate = ReturnDate ?? asOf ?? DateTime.Now;
        if (referenceDate <= DueDate)
            return 0m;

        var daysLate = (referenceDate.Date - DueDate.Date).Days;
        return daysLate * dailyFee;
    }

    public void MarkAsReturned(DateTime returnDate)
    {
        if (ReturnDate != null)
            throw new InvalidOperationException("Lånet är redan återlämnat.");

        ReturnDate = returnDate;
    }
}
