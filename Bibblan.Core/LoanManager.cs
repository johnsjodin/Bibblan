namespace Bibblan.Core;

public class LoanManager
{
    private readonly List<Loan> _loans = new List<Loan>();
    public IReadOnlyList<Loan> Loans => _loans;

    public Loan CreateLoan(Book book, Member member, DateTime loanDate, DateTime dueDate)
    {
        // Skapar lån och uppdaterar bokstatus.
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (book == null)
            throw new ArgumentNullException(nameof(book));
        if (dueDate <= DateTime.Now)
            throw new ArgumentException("Återlämningsdatum måste vara i framtiden.");
        if (book.IsReserved && book.ReservedBy != member)
            throw new InvalidOperationException("Boken är reserverad av en annan medlem.");
        var loan = new Loan(book, member, loanDate, dueDate);
        _loans.Add(loan);
        member.AddLoan(loan);
        book.MarkAsBorrowed();
        if (book.IsReserved)
            book.ClearReservation();
        return loan;
    }

    public void ReserveBook(Book book, Member member)
    {
        // Markerar boken som reserverad för medlemmen.
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        book.MarkAsReserved(member);
    }

    public bool ReturnBook(Loan loan, DateTime returnDate)
    {
        if (loan == null)
            throw new ArgumentNullException(nameof(loan));
        if (!_loans.Contains(loan))
            throw new ArgumentException("Lånet hittades inte i systemet.");
        if (loan.IsReturned)
            throw new InvalidOperationException("Detta lån har redan återlämnats.");
        loan.MarkAsReturned(returnDate);
        loan.Book.MarkAsReturned();
        return true;
    }

    public IEnumerable<Loan> GetActiveLoans()
    {
        return _loans.Where(loan => !loan.IsReturned);
    }
}
