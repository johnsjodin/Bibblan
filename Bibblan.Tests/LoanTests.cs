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
        var dueDate = loanDate.AddDays(7); // Due date is 3 days ago
        var loan = new Loan(book, member, loanDate, dueDate);

        // Act
        var isOverdue = loan.IsOverdue();

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
        var dueDate = loanDate.AddDays(7); // Due date is in 4 days
        var loan = new Loan(book, member, loanDate, dueDate);

        // Act
        var isOverdue = loan.IsOverdue();

        // Assert
        Assert.False(isOverdue);
    }

}
