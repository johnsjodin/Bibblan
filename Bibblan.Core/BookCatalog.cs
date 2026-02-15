namespace Bibblan.Core;

public class BookCatalog
{
    private readonly List<Book> _books = new();

    public void AddBook(Book book)
    {
        // Lägger till en bok i katalogen efter validering.
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        _books.Add(book);
    }

    public bool RemoveBook(string isbn)
    {
        // Försöker ta bort bok baserat på ISBN.
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN får inte vara tomt.", nameof(isbn));

        var book = _books.FirstOrDefault(b => b.ISBN == isbn);
        if (book == null) return false;

        return _books.Remove(book);
    }

    public IReadOnlyList<Book> GetAll() => _books;

    public IEnumerable<Book> Search(string term)
    {
        // Filtrerar böcker utifrån sökterm.
        return _books.Where(b => b.Matches(term));
    }

    public IEnumerable<Book> SortByTitle()
    {
        // Sorterar böcker alfabetiskt på titel.
        return _books.OrderBy(b => b.Title);
    }

    public IEnumerable<Book> SortByYear()
    {
        // Sorterar böcker efter utgivningsår.
        return _books.OrderBy(b => b.PublishedYear);
    }
}