namespace Bibblan.Core;

public class BookCatalog
{
    private readonly List<Book> _books = new();

    public void AddBook(Book book)
    {
        if (book == null)
            throw new ArgumentNullException(nameof(book));

        _books.Add(book);
    }

    public bool RemoveBook(string isbn)
    {
        var book = _books.FirstOrDefault(b => b.ISBN == isbn);
        if (book == null) return false;

        return _books.Remove(book);
    }

    public IReadOnlyList<Book> GetAll() => _books;

    public IEnumerable<Book> Search(string term)
    {
        return _books.Where(b => b.Matches(term));
    }

    public IEnumerable<Book> SortByTitle()
    {
        return _books.OrderBy(b => b.Title);
    }

    public IEnumerable<Book> SortByYear()
    {
        return _books.OrderBy(b => b.PublishedYear);
    }
}