namespace Bibblan.Core;

public class LoanManager
{
    public List<Loan> Loans { get; } = new List<Loan>();

    public Loan CreateLoan(Book book, Member member, DateTime loanDate, DateTime dueDate)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (book == null)
            throw new ArgumentNullException(nameof(book));
        if (dueDate <= DateTime.Now)
            throw new ArgumentException("Återlämningsdatum måste vara i framtiden.", nameof(dueDate));
        var loan = new Loan(book, member, loanDate, dueDate);
        Loans.Add(loan);
        member.Loans.Add(loan);
        book.MarkAsBorrowed();
        return loan;
    }

    public bool ReturnBook(Loan loan, DateTime returnDate)
    {
        if (loan == null)
            throw new ArgumentNullException(nameof(loan));
        if (!Loans.Contains(loan))
            throw new ArgumentException("Lånet hittades inte i systemet.", nameof(loan));
        if (loan.IsReturned())
            throw new InvalidOperationException("Detta lån har redan återlämnats.");
        loan.MarkAsReturned(returnDate);
        return true;
    }

    public IEnumerable<Loan> GetActiveLoans()
    {
        return Loans.Where(loan => !loan.IsReturned());
    }
}
