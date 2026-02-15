using Bibblan.Core;
namespace Bibblan.Tests;

public class LoanTests
{
    [Fact]
    public void IsOverdue_ShouldReturnTrue_WhenLoanIsOverdue()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-10);
        var dueDate = loanDate.AddDays(7); // Återlämningsdatum var för 3 dagar sedan
        var loan = new Loan(book, member, loanDate, dueDate);

        // Act
        var isOverdue = loan.IsOverdue;

        // Assert
        Assert.True(isOverdue);
    }

    [Fact]
    public void IsOverdue_ShouldReturnFalse_WhenLoanIsNotOverdue()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-3);
        var dueDate = loanDate.AddDays(7); // Återlämningsdatum är om 4 dagar
        var loan = new Loan(book, member, loanDate, dueDate);

        // Act
        var isOverdue = loan.IsOverdue;

        // Assert
        Assert.False(isOverdue);
    }

    [Fact]
    public void IsReturned_ShouldReturnFalse_WhenLoanIsNotReturned()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-3);
        var dueDate = loanDate.AddDays(7);
        var loan = new Loan(book, member, loanDate, dueDate);

        // Act
        var isReturned = loan.IsReturned;

        // Assert
        Assert.False(isReturned);
    }

    [Fact]
    public void IsReturned_ShouldReturnTrue_WhenLoanIsReturned()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-10);
        var dueDate = loanDate.AddDays(7);
        var loan = new Loan(book, member, loanDate, dueDate);

        // Act
        loan.MarkAsReturned(DateTime.Now);
        var isReturned = loan.IsReturned;

        // Assert
        Assert.True(isReturned);
    }

    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);

        // Act
        var loan = new Loan(book, member, loanDate, dueDate);

        // Assert
        Assert.Equal(book, loan.Book);
        Assert.Equal(member, loan.Member);
        Assert.Equal(loanDate, loan.LoanDate);
        Assert.Equal(dueDate, loan.DueDate);
        Assert.Null(loan.ReturnDate);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenBookIsNull()
    {
        // Arrange
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Loan(null, member, loanDate, dueDate));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenMemberIsNull()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Loan(book, null, loanDate, dueDate));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenLoanDateIsAfterDueDate()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(10);
        var dueDate = DateTime.Now; // Återlämningsdatum är före lånedatumet

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Loan(book, member, loanDate, dueDate));
    }

    [Fact]
    public void MarkAsReturned_ShouldSetReturnDate()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);
        var loan = new Loan(book, member, loanDate, dueDate);
        var beforeReturn = DateTime.Now;

        // Act
        loan.MarkAsReturned(DateTime.Now);

        // Assert
        Assert.NotNull(loan.ReturnDate);
        Assert.True(loan.ReturnDate >= beforeReturn);
    }

    [Fact]
    public void MarkAsReturned_ShouldThrowException_WhenAlreadyReturned()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);
        var loan = new Loan(book, member, loanDate, dueDate);
        loan.MarkAsReturned(DateTime.Now);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => loan.MarkAsReturned(DateTime.Now));
    }

    [Fact]
    public void IsOverdue_ShouldReturnFalse_WhenLoanIsReturned()
    {
        // Arrange
        var book = new Book("123", "Titel", "Författare", 2020);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-10);
        var dueDate = loanDate.AddDays(7); // Återlämningsdatum var för 3 dagar sedan
        var loan = new Loan(book, member, loanDate, dueDate);
        loan.MarkAsReturned(DateTime.Now);

        // Act
        var isOverdue = loan.IsOverdue;

        // Assert
        Assert.False(isOverdue); // Återlämnade lån ska inte vara försenade
    }
}