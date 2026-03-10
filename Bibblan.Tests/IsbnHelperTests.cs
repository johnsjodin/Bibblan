using Bibblan.Data.Validation;

namespace Bibblan.Tests;

/// <summary>
/// Tester för ISBN-validering och formatering.
/// </summary>
public class IsbnHelperTests
{
    [Theory]
    [InlineData("9789100123456", true)]   // ISBN-13 utan bindestreck
    [InlineData("978-91-0-012345-6", true)] // ISBN-13 med bindestreck
    [InlineData("9100123456", true)]       // ISBN-10 utan bindestreck
    [InlineData("91-0-012345-6", true)]    // ISBN-10 med bindestreck
    [InlineData("123456789", false)]       // För kort
    [InlineData("12345678901234", false)]  // För lång
    [InlineData("", false)]                // Tom
    [InlineData("abcdefghij", false)]      // Bara bokstäver
    public void IsValid_ShouldValidateCorrectly(string isbn, bool expected)
    {
        // Act
        var result = IsbnHelper.IsValid(isbn);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetDigitsOnly_ShouldRemoveNonDigits()
    {
        // Arrange
        var isbn = "978-91-0-012345-6";

        // Act
        var result = IsbnHelper.GetDigitsOnly(isbn);

        // Assert
        Assert.Equal("9789100123456", result);
    }

    [Fact]
    public void Format_ShouldFormatIsbn13Correctly()
    {
        // Arrange
        var isbn = "9789100123456";

        // Act
        var result = IsbnHelper.Format(isbn);

        // Assert
        Assert.Equal("978-91-0-012345-6", result);
    }

    [Fact]
    public void Format_ShouldFormatIsbn10Correctly()
    {
        // Arrange
        var isbn = "9100123456";

        // Act
        var result = IsbnHelper.Format(isbn);

        // Assert
        Assert.Equal("91-0-012345-6", result);
    }

    [Fact]
    public void Format_ShouldHandleAlreadyFormattedIsbn()
    {
        // Arrange - ISBN som redan har bindestreck
        var isbn = "978-91-0-012345-6";

        // Act
        var result = IsbnHelper.Format(isbn);

        // Assert - Ska fortfarande ge korrekt format
        Assert.Equal("978-91-0-012345-6", result);
    }

    [Fact]
    public void Format_ShouldReturnOriginalForInvalidLength()
    {
        // Arrange
        var isbn = "12345";

        // Act
        var result = IsbnHelper.Format(isbn);

        // Assert - Returnerar original för ogiltigt ISBN
        Assert.Equal("12345", result);
    }
}
