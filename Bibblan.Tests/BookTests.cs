using Bibblan.Core;
namespace Bibblan.Tests;

public class BookTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 2024);

        // Assert
        Assert.Equal("978-91-0-012345-6", book.ISBN);
        Assert.Equal("Testbok", book.Title);
        Assert.Equal("Testförfattare", book.Author);
        Assert.Equal(2024, book.PublishedYear);
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIsbnIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book("", "Titel", "Författare", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTitleIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book("123", "", "Författare", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenAuthorIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book("123", "Titel", "", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenPublishedYearIsNegative()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book("123", "Titel", "Författare", -1));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenPublishedYearIsInFuture()
    {
        // Arrange & Act & Assert
        var futureYear = DateTime.Now.Year + 1;
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book("123", "Titel", "Författare", futureYear));
    }

    [Fact]
    public void IsAvailable_ShouldBeTrueForNewBook()
    {
        // Arrange & Act
        var book = new Book("123", "Titel", "Författare", 2020);

        // Assert
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void GetInfo_ShouldReturnFormattedString()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);

        // Act
        var info = book.GetInfo();

        // Assert
        Assert.Equal("\"Titel\" av Författare (2020) - Tillgänglig", info);
    }

    [Fact]
    public void GetInfo_ShouldShowUtlånad_WhenBookIsBorrowed()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act
        var info = book.GetInfo();

        // Assert
        Assert.Equal("\"Titel\" av Författare (2020) - Utlånad", info);
    }

    [Theory]
    [InlineData("Tolkien", true)]
    [InlineData("tolkien", true)] // case-insensitive
    [InlineData("Rowling", false)]
    public void Matches_ShouldFindByAuthor(string searchTerm, bool expected)
    {
        // Arrange
        var book = new Book("123", "Sagan om ringen", "J.R.R. Tolkien", 1954);

        // Act
        var result = book.Matches(searchTerm);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Matches_ShouldFindByTitle()
    {
        // Arrange
        var book = new Book("123", "Sagan om ringen", "J.R.R. Tolkien", 1954);

        // Act
        var result = book.Matches("Sagan");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Matches_ShouldFindByISBN()
    {
        // Arrange
        var book = new Book("978-91-0-012345-6", "Titel", "Författare", 2020);

        // Act
        var result = book.Matches("978-91");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsNull()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);

        // Act
        var result = book.Matches(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsEmpty()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);

        // Act
        var result = book.Matches("");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsWhitespace()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);

        // Act
        var result = book.Matches("   ");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MarkAsBorrowed_ShouldSetIsAvailableToFalse()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);

        // Act
        book.MarkAsBorrowed();

        // Assert
        Assert.False(book.IsAvailable);
    }

    [Fact]
    public void MarkAsBorrowed_ShouldThrowException_WhenAlreadyBorrowed()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => book.MarkAsBorrowed());
    }

    [Fact]
    public void MarkAsReturned_ShouldSetIsAvailableToTrue()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act
        book.MarkAsReturned();

        // Assert
        Assert.True(book.IsAvailable);
    }
}
