using Bibblan.Core;
namespace Bibblan.Tests;

public class LibraryTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();

        // Act
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        // Assert
        Assert.Equal(bookCatalog, library.BookCatalog);
        Assert.Equal(memberRegistry, library.MemberRegistry);
        Assert.Equal(loanManager, library.LoanManager);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenBookCatalogIsNull()
    {
        // Arrange
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Library(null, memberRegistry, loanManager));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenMemberRegistryIsNull()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var loanManager = new LoanManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Library(bookCatalog, null, loanManager));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenLoanManagerIsNull()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Library(bookCatalog, memberRegistry, null));
    }

    [Fact]
    public void SearchBooks_ShouldReturnMatchingBooks()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var book3 = new Book("789", "Hobbit", "J.R.R. Tolkien", 1937);
        bookCatalog.AddBook(book1);
        bookCatalog.AddBook(book2);
        bookCatalog.AddBook(book3);

        // Act
        var results = library.SearchBooks("Tolkien").ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Contains(book2, results);
        Assert.Contains(book3, results);
    }

    [Fact]
    public void SearchBooks_ShouldReturnEmpty_WhenNoMatchesFound()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        bookCatalog.AddBook(book);

        // Act
        var results = library.SearchBooks("Tolkien").ToList();

        // Assert
        Assert.Empty(results);
    }

    // Tester för statistikmetoderna

    [Fact]
    public void GetTotalBooks_ShouldReturnZero_WhenCatalogIsEmpty()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        // Act
        var total = library.GetTotalBooks();

        // Assert
        Assert.Equal(0, total);
    }

    [Fact]
    public void GetTotalBooks_ShouldReturnCorrectCount()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var book3 = new Book("789", "Hobbit", "J.R.R. Tolkien", 1937);
        bookCatalog.AddBook(book1);
        bookCatalog.AddBook(book2);
        bookCatalog.AddBook(book3);

        // Act
        var total = library.GetTotalBooks();

        // Assert
        Assert.Equal(3, total);
    }

    [Fact]
    public void GetBorrowedBooksCount_ShouldReturnZero_WhenNoActiveLoans()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        // Act
        var count = library.GetBorrowedBooksCount();

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void GetBorrowedBooksCount_ShouldReturnCorrectCount()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);
        loanManager.CreateLoan(book1, member, loanDate, dueDate);
        loanManager.CreateLoan(book2, member, loanDate, dueDate);

        // Act
        var count = library.GetBorrowedBooksCount();

        // Assert
        Assert.Equal(2, count);
    }

    [Fact]
    public void GetBorrowedBooksCount_ShouldNotCountReturnedBooks()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);
        var loan1 = loanManager.CreateLoan(book1, member, loanDate, dueDate);
        loanManager.CreateLoan(book2, member, loanDate, dueDate);

        loanManager.ReturnBook(loan1, DateTime.Now);

        // Act
        var count = library.GetBorrowedBooksCount();

        // Assert
        Assert.Equal(1, count); // Only book2 is still borrowed
    }

    [Fact]
    public void GetMostActiveBorrower_ShouldReturnNull_WhenNoActiveLoans()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        // Act
        var mostActive = library.GetMostActiveBorrower();

        // Assert
        Assert.Null(mostActive);
    }

    [Fact]
    public void GetMostActiveBorrower_ShouldReturnMemberWithMostLoans()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var book3 = new Book("789", "Hobbit", "J.R.R. Tolkien", 1937);

        var member1 = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var member2 = new Member("67890", "Anna Andersson", "anna@testemail.se");

        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);

        loanManager.CreateLoan(book1, member1, loanDate, dueDate);
        loanManager.CreateLoan(book2, member1, loanDate, dueDate);
        loanManager.CreateLoan(book3, member2, loanDate, dueDate);

        // Act
        var mostActive = library.GetMostActiveBorrower();

        // Assert
        Assert.Equal(member1, mostActive); // member1 har 2 lån medan member2 har 1 lån
    }

    [Fact]
    public void GetMostActiveBorrower_ShouldNotCountReturnedLoans()
    {
        // Arrange
        var bookCatalog = new BookCatalog();
        var memberRegistry = new MemberRegistry();
        var loanManager = new LoanManager();
        var library = new Library(bookCatalog, memberRegistry, loanManager);

        var book1 = new Book("123", "Harry Potter", "J.K. Rowling", 1997);
        var book2 = new Book("456", "Sagan om ringen", "J.R.R. Tolkien", 1954);
        var book3 = new Book("789", "Hobbit", "J.R.R. Tolkien", 1937);

        var member1 = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var member2 = new Member("67890", "Anna Andersson", "anna@testemail.se");

        var loanDate = DateTime.Now.AddDays(-5);
        var dueDate = loanDate.AddDays(14);

        var loan1 = loanManager.CreateLoan(book1, member1, loanDate, dueDate);
        var loan2 = loanManager.CreateLoan(book2, member1, loanDate, dueDate);
        loanManager.CreateLoan(book3, member2, loanDate, dueDate);

        // Lämna tillbaka båda lån för member1 så att member2 blir den mest aktiva låntagaren
        loanManager.ReturnBook(loan1, DateTime.Now);
        loanManager.ReturnBook(loan2, DateTime.Now);

        // Act
        var mostActive = library.GetMostActiveBorrower();

        // Assert
        Assert.Equal(member2, mostActive); // member2 är nu den mest aktiva låntagaren
    }
}
