using Bibblan.Core;
namespace Bibblan.Tests;

public class BookCatalogTests
{
    [Fact]
    public void AddBook_ShouldAddBookToCatalog()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book = new Book("123", "Titel", "Författare", 2020);

        // Act
        catalog.AddBook(book);

        // Assert
        var books = catalog.GetAll();
        Assert.Single(books);
        Assert.Contains(book, books);
    }

    [Fact]
    public void AddBook_ShouldThrowException_WhenBookIsNull()
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => catalog.AddBook(null));
    }

    [Fact]
    public void AddBook_ShouldAddMultipleBooks()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Titel 1", "Författare 1", 2020);
        var book2 = new Book("456", "Titel 2", "Författare 2", 2021);
        var book3 = new Book("789", "Titel 3", "Författare 3", 2022);

        // Act
        catalog.AddBook(book1);
        catalog.AddBook(book2);
        catalog.AddBook(book3);

        // Assert
        var books = catalog.GetAll();
        Assert.Equal(3, books.Count);
    }

    [Fact]
    public void RemoveBook_ShouldReturnTrue_WhenBookExists()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book = new Book("123", "Titel", "Författare", 2020);
        catalog.AddBook(book);

        // Act
        var result = catalog.RemoveBook("123");

        // Assert
        Assert.True(result);
        Assert.Empty(catalog.GetAll());
    }

    [Fact]
    public void RemoveBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act
        var result = catalog.RemoveBook("nonexistent");

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void RemoveBook_ShouldThrowException_WhenIsbnIsInvalid(string isbn)
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => catalog.RemoveBook(isbn));
    }

    [Fact]
    public void RemoveBook_ShouldRemoveCorrectBook_WhenMultipleBooksExist()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Titel 1", "Författare 1", 2020);
        var book2 = new Book("456", "Titel 2", "Författare 2", 2021);
        var book3 = new Book("789", "Titel 3", "Författare 3", 2022);
        catalog.AddBook(book1);
        catalog.AddBook(book2);
        catalog.AddBook(book3);

        // Act
        catalog.RemoveBook("456");

        // Assert
        var books = catalog.GetAll();
        Assert.Equal(2, books.Count);
        Assert.Contains(book1, books);
        Assert.DoesNotContain(book2, books);
        Assert.Contains(book3, books);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_WhenCatalogIsEmpty()
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act
        var books = catalog.GetAll();

        // Assert
        Assert.Empty(books);
    }

    [Fact]
    public void GetAll_ShouldReturnAllBooks()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Titel 1", "Författare 1", 2020);
        var book2 = new Book("456", "Titel 2", "Författare 2", 2021);
        catalog.AddBook(book1);
        catalog.AddBook(book2);

        // Act
        var books = catalog.GetAll();

        // Assert
        Assert.Equal(2, books.Count);
        Assert.Contains(book1, books);
        Assert.Contains(book2, books);
    }

    [Fact]
    public void Search_ShouldFindBooksByTitle()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        catalog.AddBook(book1);
        catalog.AddBook(book2);

        // Act
        var results = catalog.Search("Harry").ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains(book1, results);
    }

    [Fact]
    public void Search_ShouldFindBooksByAuthor()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        catalog.AddBook(book1);
        catalog.AddBook(book2);

        // Act
        var results = catalog.Search("Tolkien").ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains(book2, results);
    }

    [Fact]
    public void Search_ShouldBeCaseInsensitive()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        catalog.AddBook(book);

        // Act
        var results = catalog.Search("harry").ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains(book, results);
    }

    [Fact]
    public void Search_ShouldReturnEmpty_WhenNoMatchesFound()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        catalog.AddBook(book);

        // Act
        var results = catalog.Search("nonexistent").ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Search_ShouldReturnEmpty_WhenCatalogIsEmpty()
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act
        var results = catalog.Search("anything").ToList();

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void SortByTitle_ShouldReturnBooksInAlphabeticalOrder()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Zorro", "Författare 1", 2020);
        var book2 = new Book("456", "Alpha", "Författare 2", 2021);
        var book3 = new Book("789", "Beta", "Författare 3", 2022);
        catalog.AddBook(book1);
        catalog.AddBook(book2);
        catalog.AddBook(book3);

        // Act
        var sorted = catalog.SortByTitle().ToList();

        // Assert
        Assert.Equal(3, sorted.Count);
        Assert.Equal(book2, sorted[0]); // Alpha
        Assert.Equal(book3, sorted[1]); // Beta
        Assert.Equal(book1, sorted[2]); // Zorro
    }

    [Fact]
    public void SortByTitle_ShouldReturnEmpty_WhenCatalogIsEmpty()
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act
        var sorted = catalog.SortByTitle().ToList();

        // Assert
        Assert.Empty(sorted);
    }

    [Fact]
    public void SortByYear_ShouldReturnBooksInChronologicalOrder()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Ny bok", "Författare 1", 2022);
        var book2 = new Book("456", "Gammal bok", "Författare 2", 1954);
        var book3 = new Book("789", "Mellanbok", "Författare 3", 2000);
        catalog.AddBook(book1);
        catalog.AddBook(book2);
        catalog.AddBook(book3);

        // Act
        var sorted = catalog.SortByYear().ToList();

        // Assert
        Assert.Equal(3, sorted.Count);
        Assert.Equal(book2, sorted[0]); // 1954
        Assert.Equal(book3, sorted[1]); // 2000
        Assert.Equal(book1, sorted[2]); // 2022
    }

    [Fact]
    public void SortByYear_ShouldReturnEmpty_WhenCatalogIsEmpty()
    {
        // Arrange
        var catalog = new BookCatalog();

        // Act
        var sorted = catalog.SortByYear().ToList();

        // Assert
        Assert.Empty(sorted);
    }

    [Fact]
    public void SortByYear_ShouldHandleSameYear()
    {
        // Arrange
        var catalog = new BookCatalog();
        var book1 = new Book("123", "Bok 1", "Författare 1", 2020);
        var book2 = new Book("456", "Bok 2", "Författare 2", 2020);
        catalog.AddBook(book1);
        catalog.AddBook(book2);

        // Act
        var sorted = catalog.SortByYear().ToList();

        // Assert
        Assert.Equal(2, sorted.Count);
        // Both books have the same year, so order is preserved
    }
}
