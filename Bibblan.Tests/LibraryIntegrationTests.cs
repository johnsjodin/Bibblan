using Bibblan.Data;
using Bibblan.Data.Entities;
using Bibblan.Data.Repositories;
using Microsoft.EntityFrameworkCore;

#pragma warning disable xUnit1051 // CancellationToken i testmetoder är inte kritiskt

namespace Bibblan.Tests;

/// <summary>
/// Integrationstester för affärslogik med Entity Framework.
/// </summary>
public class LibraryIntegrationTests
{
    private LibraryContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new LibraryContext(options);
    }

    [Fact]
    public async Task BorrowBook_ShouldUpdateAvailability()
    {
        // Arrange - Testar att en bok blir otillgänglig vid utlåning
        using var context = CreateContext(nameof(BorrowBook_ShouldUpdateAvailability));
        var bookRepo = new BookRepository(context);
        var loanRepo = new LoanRepository(context);

        var book = new BookEntity { ISBN = "123", Title = "Bok", Author = "Författare", PublishedYear = 2020, IsAvailable = true };
        var member = new MemberEntity { MemberId = "M001", Name = "Test", Email = "test@test.se" };
        context.Books.Add(book);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        // Act - Skapa lån och uppdatera bok
        var loan = new LoanEntity
        {
            BookId = book.Id,
            MemberId = member.Id,
            LoanDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(14)
        };
        await loanRepo.AddAsync(loan);

        book.IsAvailable = false;
        await bookRepo.UpdateAsync(book);

        // Assert
        var updatedBook = await bookRepo.GetByIdAsync(book.Id);
        Assert.False(updatedBook!.IsAvailable);
    }

    [Fact]
    public async Task ReturnBook_ShouldUpdateAvailabilityAndLoan()
    {
        // Arrange - Testar att återlämning uppdaterar både bok och lån
        using var context = CreateContext(nameof(ReturnBook_ShouldUpdateAvailabilityAndLoan));
        var bookRepo = new BookRepository(context);
        var loanRepo = new LoanRepository(context);

        var book = new BookEntity { ISBN = "123", Title = "Bok", Author = "Författare", PublishedYear = 2020, IsAvailable = false };
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

        // Act - Återlämna boken
        loan.ReturnDate = DateTime.Now;
        await loanRepo.UpdateAsync(loan);

        book.IsAvailable = true;
        await bookRepo.UpdateAsync(book);

        // Assert
        var returnedLoan = await loanRepo.GetByIdAsync(loan.Id);
        var availableBook = await bookRepo.GetByIdAsync(book.Id);

        Assert.True(returnedLoan!.IsReturned);
        Assert.True(availableBook!.IsAvailable);
    }

    [Fact]
    public async Task MemberWithLoans_ShouldIncludeLoansInQuery()
    {
        // Arrange - Testar att medlem inkluderar lån via navigation property
        using var context = CreateContext(nameof(MemberWithLoans_ShouldIncludeLoansInQuery));
        var memberRepo = new MemberRepository(context);

        var book1 = new BookEntity { ISBN = "1", Title = "Bok 1", Author = "Författare", PublishedYear = 2020 };
        var book2 = new BookEntity { ISBN = "2", Title = "Bok 2", Author = "Författare", PublishedYear = 2021 };
        var member = new MemberEntity { MemberId = "M001", Name = "Aktiv låntagare", Email = "test@test.se" };
        context.Books.AddRange(book1, book2);
        context.Members.Add(member);
        await context.SaveChangesAsync();

        context.Loans.AddRange(
            new LoanEntity { BookId = book1.Id, MemberId = member.Id, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(14) },
            new LoanEntity { BookId = book2.Id, MemberId = member.Id, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(14) }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await memberRepo.GetByIdAsync(member.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Loans.Count);
    }
}
