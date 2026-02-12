using Bibblan.Core;
using Xunit;

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
