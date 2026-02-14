using Bibblan.Core;
namespace Bibblan.Tests;

public class MemberTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Assert
        Assert.Equal("12345", member.MemberId);
        Assert.Equal("Johan Johansson", member.Name);
        Assert.Equal("johan@testemail.se", member.Email);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenMemberIdIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Member("", "Johan Johansson", "johan@testemail.se"));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Member("12345", "", "johan@testemail.se"));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenEmailIsEmpty()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Member("12345", "Johan Johansson", ""));
    }

    [Fact]
    public void Constructor_ShouldSetMemberSinceToCurrentDate()
    {
        // Arrange
        var beforeCreation = DateTime.Now.Date;

        // Act
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Assert
        var afterCreation = DateTime.Now.Date;
        Assert.True(member.MemberSince.Date >= beforeCreation && member.MemberSince.Date <= afterCreation);
    }

    [Fact]
    public void Constructor_ShouldInitializeLoansAsEmptyList()
    {
        // Arrange & Act
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Assert
        Assert.NotNull(member.Loans);
        Assert.Empty(member.Loans);
    }

    [Fact]
    public void GetInfo_ShouldReturnFormattedString()
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act
        var info = member.GetInfo();

        // Assert
        Assert.Contains("Medlem: Johan Johansson", info);
        Assert.Contains("ID: 12345", info);
        Assert.Contains("E-post: johan@testemail.se", info);
        Assert.Contains("Inga lånade böcker.", info);
    }

    [Fact]
    public void GetInfo_ShouldShowLoanedBooks()
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);

        var loan1 = new Loan(book1, member, loanDate, dueDate);
        var loan2 = new Loan(book2, member, loanDate, dueDate.AddDays(7));

        member.Loans.Add(loan1);
        member.Loans.Add(loan2);

        // Act
        var info = member.GetInfo();

        // Assert
        Assert.Contains("Harry Potter", info);
        Assert.Contains("J.K. Rowling", info);
        Assert.Contains("Sagan om ringen", info);
        Assert.Contains("J.R.R. Tolkien", info);
        Assert.Contains("Återlämnas:", info);
        Assert.DoesNotContain("Inga lånade böcker.", info);
    }

    [Theory]
    [InlineData("Johan", true)]
    [InlineData("johan", true)] // case-insensitive
    [InlineData("testemail", true)]
    [InlineData("12345", true)]
    [InlineData("nonexistent", false)]
    public void Matches_ShouldFindByNameEmailOrId(string searchTerm, bool expected)
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act
        var result = member.Matches(searchTerm);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsNull()
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act
        var result = member.Matches(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsEmpty()
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act
        var result = member.Matches("");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Matches_ShouldReturnFalse_WhenSearchTermIsWhitespace()
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act
        var result = member.Matches("   ");

        // Assert
        Assert.False(result);
    }
}