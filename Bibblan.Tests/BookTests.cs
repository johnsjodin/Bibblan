using Bibblan.Core;
namespace Bibblan.Tests;

public class BookTests
{
    // Standard test-ISBN för att användas i tester
    private const string ValidIsbn13 = "9789100123456";
    private const string ValidIsbn10 = "9100123456";

    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var book = new Book("978-91-0-012345-6", "Testbok", "Testförfattare", 2024);

        // Assert - ISBN lagras som siffror
        Assert.Equal("9789100123456", book.ISBN);
        Assert.Equal("Testbok", book.Title);
        Assert.Equal("Testförfattare", book.Author);
        Assert.Equal(2024, book.PublishedYear);
        Assert.True(book.IsAvailable);
        Assert.False(book.IsReserved);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIsbnIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book("", "Titel", "Författare", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenIsbnIsTooShort()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book("123456789", "Titel", "Författare", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenTitleIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book(ValidIsbn13, "", "Författare", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenAuthorIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Book(ValidIsbn13, "Titel", "", 2020));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenPublishedYearIsNegative()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book(ValidIsbn13, "Titel", "Författare", -1));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenPublishedYearIsInFuture()
    {
        // Arrange & Act & Assert
        var futureYear = DateTime.Now.Year + 2;
        Assert.Throws<ArgumentOutOfRangeException>(() => new Book(ValidIsbn13, "Titel", "Författare", futureYear));
    }

    [Fact]
    public void IsAvailable_ShouldBeTrueForNewBook()
    {
        // Arrange & Act
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);

        // Assert
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void GetInfo_ShouldReturnFormattedString()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);

        // Act
        var info = book.GetInfo();

        // Assert - FormattedISBN används nu
        Assert.Equal("\"Titel\" av Författare (ISBN: 978-91-0-012345-6) (2020) - Tillgänglig", info);
    }

    [Fact]
    public void GetInfo_ShouldShowUtlånad_WhenBookIsBorrowed()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act
        var info = book.GetInfo();

        // Assert
        Assert.Equal("\"Titel\" av Författare (ISBN: 978-91-0-012345-6) (2020) - Utlånad", info);
    }

    [Fact]
    public void MarkAsReserved_ShouldSucceed_WhenBookIsBorrowed()
    {
        // Arrange - Nu kan man reservera utlånade böcker
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        book.MarkAsBorrowed();

        // Act
        book.MarkAsReserved(member);

        // Assert
        Assert.True(book.IsReserved);
        Assert.Equal(member, book.ReservedBy);
    }

    [Fact]
    public void GetInfo_ShouldShowReserverad_WhenBookIsReserved()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        book.MarkAsBorrowed(); // Boken måste vara utlånad för att reserveras
        book.MarkAsReserved(member);

        // Act
        var info = book.GetInfo();

        // Assert
        Assert.Equal("\"Titel\" av Författare (ISBN: 978-91-0-012345-6) (2020) - Utlånad (Reserverad)", info);
    }

    [Fact]
    public void MarkAsReserved_ShouldSetReservationState()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        book.MarkAsBorrowed(); // Boken måste vara utlånad för att reserveras

        // Act
        book.MarkAsReserved(member);

        // Assert
        Assert.True(book.IsReserved);
        Assert.Equal(member, book.ReservedBy);
    }

    [Fact]
    public void MarkAsReserved_ShouldThrowException_WhenMemberIsNull()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => book.MarkAsReserved(null));
    }

    [Fact]
    public void MarkAsReserved_ShouldThrowException_WhenAlreadyReserved()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        book.MarkAsBorrowed();
        book.MarkAsReserved(member);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => book.MarkAsReserved(member));
    }

    [Fact]
    public void MarkAsReserved_ShouldThrowException_WhenBookIsAvailable()
    {
        // Arrange - boken är tillgänglig (kan lånas direkt)
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => book.MarkAsReserved(member));
        Assert.Contains("tillgänglig", ex.Message);
    }

    [Fact]
    public void ClearReservation_ShouldResetReservationState()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        book.MarkAsBorrowed();
        book.MarkAsReserved(member);

        // Act
        book.ClearReservation();

        // Assert
        Assert.False(book.IsReserved);
        Assert.Null(book.ReservedBy);
    }

    [Theory]
    [InlineData("Tolkien", true)]
    [InlineData("tolkien", true)] // case-insensitive
    [InlineData("Rowling", false)]
    public void Matches_ShouldFindByAuthor(string searchTerm, bool expected)
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Sagan om ringen", "J.R.R. Tolkien", 1954);

        // Act
        var result = book.Matches(searchTerm);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Matches_ShouldFindByTitle()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Sagan om ringen", "J.R.R. Tolkien", 1954);

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

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("   ")]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsInvalid(string? searchTerm)
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);

        // Act
        var result = book.Matches(searchTerm);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MarkAsBorrowed_ShouldSetIsAvailableToFalse()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);

        // Act
        book.MarkAsBorrowed();

        // Assert
        Assert.False(book.IsAvailable);
    }

    [Fact]
    public void MarkAsBorrowed_ShouldThrowException_WhenAlreadyBorrowed()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => book.MarkAsBorrowed());
    }

    [Fact]
    public void MarkAsReturned_ShouldSetIsAvailableToTrue()
    {
        // Arrange
        var book = new Book(ValidIsbn13, "Titel", "Författare", 2020);
        book.MarkAsBorrowed();

        // Act
        book.MarkAsReturned();

        // Assert
        Assert.True(book.IsAvailable);
    }

    [Fact]
    public void FormattedISBN_ShouldFormatIsbn13Correctly()
    {
        // Arrange
        var book = new Book("9789100123456", "Titel", "Författare", 2020);

        // Act & Assert
        Assert.Equal("978-91-0-012345-6", book.FormattedISBN);
    }

    [Fact]
    public void FormattedISBN_ShouldFormatIsbn10Correctly()
    {
        // Arrange
        var book = new Book("9100123456", "Titel", "Författare", 2020);

        // Act & Assert
        Assert.Equal("91-0-012345-6", book.FormattedISBN);
    }
}
