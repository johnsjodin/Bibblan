using Bibblan.Core;
namespace Bibblan.Tests;

public class LoanManagerTests
{
    [Fact]
    public void CreateLoan_ShouldCreateLoanSuccessfully()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);

        // Assert
        Assert.NotNull(loan);
        Assert.Equal(book, loan.Book);
        Assert.Equal(member, loan.Member);
        Assert.Equal(loanDate, loan.LoanDate);
        Assert.Equal(dueDate, loan.DueDate);
        Assert.False(book.IsAvailable);
        Assert.Contains(loan, loanManager.Loans);
        Assert.Contains(loan, member.Loans);
    }

    [Fact]
    public void CreateLoan_ShouldThrowException_WhenBookIsNull()
    {
        // Arrange
        var loanManager = new LoanManager();
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            loanManager.CreateLoan(null, member, loanDate, dueDate));
    }

    [Fact]
    public void CreateLoan_ShouldThrowException_WhenMemberIsNull()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            loanManager.CreateLoan(book, null, loanDate, dueDate));
    }

    [Fact]
    public void CreateLoan_ShouldThrowException_WhenDueDateIsInPast()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = DateTime.Now.AddDays(-1); // I det förflutna

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            loanManager.CreateLoan(book, member, loanDate, dueDate));
    }

    [Fact]
    public void CreateLoan_ShouldMarkBookAsUnavailable()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act
        loanManager.CreateLoan(book, member, loanDate, dueDate);

        // Assert
        Assert.False(book.IsAvailable);
    }

    [Fact]
    public void CreateLoan_ShouldAddLoanToMemberLoans()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Act
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);

        // Assert
        Assert.Contains(loan, member.Loans);
    }

    [Fact]
    public void ReturnBook_ShouldReturnLateFeeAndMarkLoanAsReturned()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-7);
        var dueDate = loanDate.AddDays(14);
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);
        var returnDate = DateTime.Now;

        // Act
        var result = loanManager.ReturnBook(loan, returnDate);

        // Assert
        Assert.Equal(0m, result);
        Assert.True(loan.IsReturned);
        Assert.Equal(returnDate, loan.ReturnDate);
        Assert.True(book.IsAvailable); // Boken ska vara tillgänglig igen
    }

    [Fact]
    public void ReturnBook_ShouldThrowException_WhenLoanIsNull()
    {
        // Arrange
        var loanManager = new LoanManager();
        var returnDate = DateTime.Now;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            loanManager.ReturnBook(null!, returnDate));
    }

    [Fact]
    public void ReturnBook_ShouldThrowException_WhenLoanNotInSystem()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-7);
        var dueDate = loanDate.AddDays(14);
        var loan = new Loan(book, member, loanDate, dueDate); // Ej skapad via LoanManager
        var returnDate = DateTime.Now;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            loanManager.ReturnBook(loan, returnDate));
    }

    [Fact]
    public void ReturnBook_ShouldThrowException_WhenLoanAlreadyReturned()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-7);
        var dueDate = loanDate.AddDays(14);
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);
        var returnDate = DateTime.Now;
        loanManager.ReturnBook(loan, returnDate);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            loanManager.ReturnBook(loan, returnDate));
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(3, 30)]
    [InlineData(7, 70)]
    public void ReturnBook_ShouldReturnLateFee_WhenReturnedAfterDueDate(int daysLate, int expectedFee)
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(10);
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);
        var returnDate = dueDate.AddDays(daysLate);

        // Act
        var fee = loanManager.ReturnBook(loan, returnDate);

        // Assert
        Assert.Equal(expectedFee, decimal.ToInt32(fee));
    }

    [Fact]
    public void ReturnBook_ShouldReturnZero_WhenReturnedBeforeDueDate()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(10);
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);
        var returnDate = new DateTime(2024, 1, 5);

        // Act
        var fee = loanManager.ReturnBook(loan, returnDate);

        // Assert
        Assert.Equal(0m, fee);
    }

    [Fact]
    public void GetActiveLoans_ShouldReturnOnlyUnreturnedLoans()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book1 = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book(TestData.Isbn13_2, "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-7);
        var dueDate = loanDate.AddDays(14);

        var loan1 = loanManager.CreateLoan(book1, member, loanDate, dueDate);
        var loan2 = loanManager.CreateLoan(book2, member, loanDate, dueDate);

        loanManager.ReturnBook(loan1, DateTime.Now);

        // Act
        var activeLoans = loanManager.GetActiveLoans().ToList();

        // Assert
        Assert.Single(activeLoans);
        Assert.Contains(loan2, activeLoans);
        Assert.DoesNotContain(loan1, activeLoans);
    }

    [Fact]
    public void GetActiveLoans_ShouldReturnEmpty_WhenAllLoansReturned()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-7);
        var dueDate = loanDate.AddDays(14);
        var loan = loanManager.CreateLoan(book, member, loanDate, dueDate);
        loanManager.ReturnBook(loan, DateTime.Now);

        // Act
        var activeLoans = loanManager.GetActiveLoans().ToList();

        // Assert
        Assert.Empty(activeLoans);
    }

    [Fact]
    public void GetActiveLoans_ShouldReturnEmpty_WhenNoLoans()
    {
        // Arrange
        var loanManager = new LoanManager();

        // Act
        var activeLoans = loanManager.GetActiveLoans().ToList();

        // Assert
        Assert.Empty(activeLoans);
    }

    [Fact]
    public void GetActiveLoans_ShouldReturnAllLoans_WhenNoneReturned()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book1 = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book(TestData.Isbn13_2, "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now.AddDays(-7);
        var dueDate = loanDate.AddDays(14);

        var loan1 = loanManager.CreateLoan(book1, member, loanDate, dueDate);
        var loan2 = loanManager.CreateLoan(book2, member, loanDate, dueDate);

        // Act
        var activeLoans = loanManager.GetActiveLoans().ToList();

        // Assert
        Assert.Equal(2, activeLoans.Count);
        Assert.Contains(loan1, activeLoans);
        Assert.Contains(loan2, activeLoans);
    }

    [Fact]
    public void ReserveBook_ShouldMarkBookAsReserved()
    {
        // Arrange - Boken måste vara utlånad för att kunna reserveras
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var borrower = new Member("11111", "Kalle Karlsson", "kalle@testemail.se");
        var reserver = new Member("12345", "Johan Johansson", "johan@testemail.se");
        loanManager.CreateLoan(book, borrower, DateTime.Now, DateTime.Now.AddDays(14));

        // Act
        loanManager.ReserveBook(book, reserver);

        // Assert
        Assert.True(book.IsReserved);
        Assert.Equal(reserver, book.ReservedBy);
    }

    [Fact]
    public void ReserveBook_ShouldThrowException_WhenBookIsNull()
    {
        // Arrange
        var loanManager = new LoanManager();
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => loanManager.ReserveBook(null!, member));
    }

    [Fact]
    public void ReserveBook_ShouldThrowException_WhenMemberIsNull()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => loanManager.ReserveBook(book, null!));
    }

    [Fact]
    public void ReserveBook_ShouldThrowException_WhenBookIsAvailable()
    {
        // Arrange - Tillgängliga böcker kan lånas direkt, inte reserveras
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => loanManager.ReserveBook(book, member));
        Assert.Contains("tillgänglig", ex.Message);
    }

    [Fact]
    public void CreateLoan_ShouldThrowException_WhenBookReservedByOtherMember()
    {
        // Arrange - En bok som är reserverad ska inte kunna lånas ut till annan medlem
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var borrower = new Member("11111", "Kalle Karlsson", "kalle@testemail.se");
        var reserver = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var anotherPerson = new Member("67890", "Anna Andersson", "anna@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Låna ut boken först, reservera den, sedan återlämna
        loanManager.CreateLoan(book, borrower, loanDate, dueDate);
        loanManager.ReserveBook(book, reserver);
        loanManager.ReturnBook(loanManager.Loans.First(), DateTime.Now);

        // Act & Assert - Anna kan inte låna boken som är reserverad av Johan
        Assert.Throws<InvalidOperationException>(() => loanManager.CreateLoan(book, anotherPerson, loanDate, dueDate.AddDays(14)));
    }

    [Fact]
    public void CreateLoan_ShouldClearReservation_WhenReservedBySameMember()
    {
        // Arrange
        var loanManager = new LoanManager();
        var book = new Book(TestData.Isbn13_1, "Harry Potter", "J.K. Rowling", 1997);
        var borrower = new Member("11111", "Kalle Karlsson", "kalle@testemail.se");
        var reserver = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var loanDate = DateTime.Now;
        var dueDate = loanDate.AddDays(14);

        // Låna ut, reservera, återlämna
        loanManager.CreateLoan(book, borrower, loanDate, dueDate);
        loanManager.ReserveBook(book, reserver);
        loanManager.ReturnBook(loanManager.Loans.First(), DateTime.Now);

        // Act - Nu lånar den som reserverade
        loanManager.CreateLoan(book, reserver, loanDate, dueDate.AddDays(14));

        // Assert - Reservationen ska rensas
        Assert.False(book.IsReserved);
        Assert.Null(book.ReservedBy);
    }
}
