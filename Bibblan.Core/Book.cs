using System.Text.RegularExpressions;

namespace Bibblan.Core;

public partial class Book : ISearchable
{
    public string ISBN { get; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int PublishedYear { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsReserved { get; private set; }
    public Member? ReservedBy { get; private set; }

    /// <summary>
    /// Returnerar ISBN formaterat med bindestreck för läsbarhet.
    /// </summary>
    public string FormattedISBN => FormatIsbn(ISBN);

    public Book(string isbn, string title, string author, int publishedYear)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN får inte vara tomt.", nameof(isbn));

        // Validera ISBN-längd (exakt 10 eller 13 siffror)
        var digitsOnly = GetDigitsOnly(isbn);
        if (digitsOnly.Length != 10 && digitsOnly.Length != 13)
            throw new ArgumentException("ISBN måste vara exakt 10 eller 13 siffror.", nameof(isbn));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Titel får inte vara tom.", nameof(title));

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Författare får inte vara tom.", nameof(author));

        if (publishedYear < 1450 || publishedYear > DateTime.Now.Year + 1)
            throw new ArgumentOutOfRangeException(nameof(publishedYear), "Utgivningsår måste vara mellan 1450 och nästa år.");

        ISBN = digitsOnly; // Spara endast siffror
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
        return $"\"{Title}\" av {Author} (ISBN: {FormattedISBN}) ({PublishedYear}) - {status}";
    }

    public bool Matches(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        return Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || ISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || FormattedISBN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
    }

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
        if (!IsAvailable)
            throw new InvalidOperationException("Boken är utlånad.");
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

    // Hjälpmetoder för ISBN-hantering

    private static string GetDigitsOnly(string isbn)
    {
        return DigitsOnlyRegex().Replace(isbn, "");
    }

    private static string FormatIsbn(string digits)
    {
        return digits.Length switch
        {
            13 => $"{digits[..3]}-{digits[3..5]}-{digits[5]}-{digits[6..12]}-{digits[12]}",
            10 => $"{digits[..2]}-{digits[2]}-{digits[3..9]}-{digits[9]}",
            _ => digits
        };
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex DigitsOnlyRegex();
}
