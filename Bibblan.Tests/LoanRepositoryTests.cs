using Bibblan.Data;
using Bibblan.Data.Entities;
using Bibblan.Data.Repositories;
using Microsoft.EntityFrameworkCore;

#pragma warning disable xUnit1051 // CancellationToken i testmetoder är inte kritiskt

namespace Bibblan.Tests;

/// <summary>
/// Tester för LoanRepository och integration mellan entiteter.
/// </summary>
public class LoanRepositoryTests
{
    private LibraryContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new LibraryContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldCreateLoan()
    {
        // Arrange
        using var context = CreateContext(nameof(AddAsync_ShouldCreateLoan));

        var book = new BookEntity { ISBN = TestData.Isbn13_1, Title = "Bok", Author = "Författare", PublishedYear = 2020 };
        var member = new MemberEntity { MemberId = "M001", Name = "Test", Email = "test@test.se" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var repository = new LoanRepository(context);
        var loan = new LoanEntity
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14)
        };

        // Act
        await repository.AddAsync(loan);

        // Assert
        var savedLoan = await context.Loans.FirstOrDefaultAsync();
        Assert.NotNull(savedLoan);
        Assert.Equal(book.Id, savedLoan.BookId);
        Assert.Equal(member.Id, savedLoan.MemberId);
    }

    [Fact]
    public async Task GetActiveLoansAsync_ShouldReturnOnlyActiveLoans()
    {
        // Arrange
        using var context = CreateContext(nameof(GetActiveLoansAsync_ShouldReturnOnlyActiveLoans));

        var book1 = new BookEntity { ISBN = TestData.Isbn13_1, Title = "Bok 1", Author = "Författare", PublishedYear = 2020 };
        var book2 = new BookEntity { ISBN = TestData.Isbn13_2, Title = "Bok 2", Author = "Författare", PublishedYear = 2020 };
        var member = new MemberEntity { MemberId = "M001", Name = "Test", Email = "test@test.se" };
        context.Books.AddRange(book1, book2);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        // Aktivt lån
        context.Loans.Add(new LoanEntity
        {
            BookId = book1.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Now.AddDays(-7),
            DueDate = DateTime.Now.AddDays(7),
            ReturnDate = null
        });

        // Återlämnat lån
        context.Loans.Add(new LoanEntity
        {
            BookId = book2.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Now.AddDays(-14),
            DueDate = DateTime.Now.AddDays(-7),
            ReturnDate = DateTime.Now.AddDays(-5)
        });
        await context.SaveChangesAsync();

        var repository = new LoanRepository(context);

        // Act
        var activeLoans = await repository.GetActiveLoansAsync();

        // Assert
        Assert.Single(activeLoans);
    }

    [Fact]
    public async Task GetOverdueLoansAsync_ShouldReturnOverdueLoans()
    {
        // Arrange
        using var context = CreateContext(nameof(GetOverdueLoansAsync_ShouldReturnOverdueLoans));

        var book = new BookEntity { ISBN = TestData.Isbn13_1, Title = "Bok", Author = "Författare", PublishedYear = 2020 };
        var member = new MemberEntity { MemberId = "M001", Name = "Test", Email = "test@test.se" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        // Försenat lån
        context.Loans.Add(new LoanEntity
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Now.AddDays(-21),
            DueDate = DateTime.Now.AddDays(-7),  // Förfallodatum passerat
            ReturnDate = null
        });
        await context.SaveChangesAsync();

        var repository = new LoanRepository(context);

        // Act
        var overdueLoans = await repository.GetOverdueLoansAsync();

        // Assert
        Assert.Single(overdueLoans);
    }

    [Fact]
    public async Task UpdateAsync_ShouldMarkLoanAsReturned()
    {
        // Arrange
        using var context = CreateContext(nameof(UpdateAsync_ShouldMarkLoanAsReturned));

        var book = new BookEntity { ISBN = TestData.Isbn13_1, Title = "Bok", Author = "Författare", PublishedYear = 2020 };
        var member = new MemberEntity { MemberId = "M001", Name = "Test", Email = "test@test.se" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var loan = new LoanEntity
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Now.AddDays(-7),
            DueDate = DateTime.Now.AddDays(7)
        };
        context.Loans.Add(loan);
        await context.SaveChangesAsync();

        var repository = new LoanRepository(context);

        // Act
        loan.ReturnDate = DateTime.Now;
        await repository.UpdateAsync(loan);

        // Assert
        var updatedLoan = await context.Loans.FindAsync(loan.Id);
        Assert.NotNull(updatedLoan!.ReturnDate);
        Assert.True(updatedLoan.IsReturned);
    }
}
