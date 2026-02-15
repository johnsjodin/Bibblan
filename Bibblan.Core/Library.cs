namespace Bibblan.Core;

public class Library
{
    public BookCatalog BookCatalog { get; }
    public MemberRegistry MemberRegistry { get; }
    public LoanManager LoanManager { get; }

    public Library(BookCatalog bookCatalog, MemberRegistry memberRegistry, LoanManager loanManager)
    {
        // Samlar systemets huvudkomponenter bakom en fasad.
        BookCatalog = bookCatalog ?? throw new ArgumentNullException(nameof(bookCatalog));
        MemberRegistry = memberRegistry ?? throw new ArgumentNullException(nameof(memberRegistry));
        LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
    }

    // ---------- SÖK ----------
    public IEnumerable<Book> SearchBooks(string term)
    {
        // Delegerar sökningar till katalogen.
        return BookCatalog.Search(term);
    }

    // ---------- STATISTIK ----------
    public int GetTotalBooks()
    {
        // Räknar alla böcker i katalogen.
        return BookCatalog.GetAll().Count;
    }

    public int GetBorrowedBooksCount()
    {
        // Räknar unika böcker som har aktiva lån.
        return LoanManager
            .GetActiveLoans()
            .Select(l => l.Book)
            .Distinct()
            .Count();
    }

    public Member? GetMostActiveBorrower()
    {
        // Hittar medlem med flest aktiva lån.
        return LoanManager
            .GetActiveLoans()
            .GroupBy(l => l.Member)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();
    }
}
