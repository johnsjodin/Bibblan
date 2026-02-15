namespace Bibblan.Core;

public class Book : ISearchable
{
    public string ISBN { get; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int PublishedYear { get; private set; }
    public bool IsAvailable { get; private set; }
    // Bokreservationens status och vem som reserverat.
    public bool IsReserved { get; private set; }
    public Member? ReservedBy { get; private set; }

    public Book(string isbn, string title, string author, int publishedYear)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN får inte vara tomt.", nameof(isbn));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Titel får inte vara tom.", nameof(title));

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Författare får inte vara tom.", nameof(author));

        if (publishedYear < 0 || publishedYear > DateTime.Now.Year)
            throw new ArgumentOutOfRangeException(nameof(publishedYear), "Ogiltigt utgivningsår.");

        ISBN = isbn;
        Title = title;
        Author = author;
        PublishedYear = publishedYear;
        IsAvailable = true;
        IsReserved = false;
        ReservedBy = null;
    }

    public string GetInfo()
    {
        // Returnerar kort statusrad för utskrift.
        string status = IsAvailable
            ? (IsReserved ? "Reserverad" : "Tillgänglig")
            : (IsReserved ? "Utlånad (Reserverad)" : "Utlånad");
        return $"\"{Title}\" av {Author} (ISBN: {ISBN}) ({PublishedYear}) - {status}";
    }

    public bool Matches(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        return Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
    }

    // Metoder som LoanManager kan använda
    public void MarkAsBorrowed()
    {
        // Markerar boken som utlånad.
        if (!IsAvailable)
            throw new InvalidOperationException("Boken är redan utlånad.");

        IsAvailable = false;
    }

    public void MarkAsReturned()
    {
        IsAvailable = true;
    }

    public void MarkAsReserved(Member member)
    {
        // Reserverar boken för en viss medlem.
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        if (IsReserved)
            throw new InvalidOperationException("Boken är redan reserverad.");

        IsReserved = true;
        ReservedBy = member;
    }

    public void ClearReservation()
    {
        // Rensar reservationen när den inte längre gäller.
        IsReserved = false;
        ReservedBy = null;
    }
}
