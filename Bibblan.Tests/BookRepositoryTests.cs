using Bibblan.Data;
using Bibblan.Data.Entities;
using Bibblan.Data.Repositories;
using Microsoft.EntityFrameworkCore;

#pragma warning disable xUnit1051 // CancellationToken i testmetoder är inte kritiskt

namespace Bibblan.Tests;

/// <summary>
/// Tester för BookRepository med InMemory-databas.
/// </summary>
public class BookRepositoryTests
{
    private LibraryContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new LibraryContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldSaveBookToDatabase()
    {
        // Arrange
        using var context = CreateContext(nameof(AddAsync_ShouldSaveBookToDatabase));
        var repository = new BookRepository(context);
        var book = new BookEntity
        {
            ISBN = "978-91-0-012345-6",
            Title = "Testbok",
            Author = "Testförfattare",
            PublishedYear = 2024
        };

        // Act
        await repository.AddAsync(book);

        // Assert
        var savedBook = await context.Books.FirstOrDefaultAsync(b => b.ISBN == "978-91-0-012345-6");
        Assert.NotNull(savedBook);
        Assert.Equal("Testbok", savedBook.Title);
        Assert.Equal("Testförfattare", savedBook.Author);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectBook()
    {
        // Arrange
        using var context = CreateContext(nameof(GetByIdAsync_ShouldReturnCorrectBook));
        var book = new BookEntity
        {
            ISBN = TestData.Isbn13_1,
            Title = "En bok",
            Author = "En författare",
            PublishedYear = 2020
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        var result = await repository.GetByIdAsync(book.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("En bok", result.Title);
    }

    [Fact]
    public async Task GetByISBNAsync_ShouldReturnCorrectBook()
    {
        // Arrange
        using var context = CreateContext(nameof(GetByISBNAsync_ShouldReturnCorrectBook));
        var book = new BookEntity
        {
            ISBN = TestData.Isbn13_2,
            Title = "ISBN-testbok",
            Author = "Författare",
            PublishedYear = 2023
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        var result = await repository.GetByISBNAsync(TestData.Isbn13_2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ISBN-testbok", result.Title);
    }

    [Fact]
    public async Task SearchAsync_ShouldFindBooksByTitle()
    {
        // Arrange
        using var context = CreateContext(nameof(SearchAsync_ShouldFindBooksByTitle));
        context.Books.AddRange(
            new BookEntity { ISBN = TestData.Isbn13_1, Title = "Pippi Långstrump", Author = "Astrid Lindgren", PublishedYear = 1945 },
            new BookEntity { ISBN = TestData.Isbn13_2, Title = "Emil i Lönneberga", Author = "Astrid Lindgren", PublishedYear = 1963 },
            new BookEntity { ISBN = TestData.Isbn13_3, Title = "Harry Potter", Author = "J.K. Rowling", PublishedYear = 1997 }
        );
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        var results = await repository.SearchAsync("Pippi");

        // Assert
        Assert.Single(results);
        Assert.Equal("Pippi Långstrump", results.First().Title);
    }

    [Fact]
    public async Task SearchAsync_ShouldFindBooksByAuthor()
    {
        // Arrange
        using var context = CreateContext(nameof(SearchAsync_ShouldFindBooksByAuthor));
        context.Books.AddRange(
            new BookEntity { ISBN = TestData.Isbn13_1, Title = "Pippi", Author = "Astrid Lindgren", PublishedYear = 1945 },
            new BookEntity { ISBN = TestData.Isbn13_2, Title = "Emil", Author = "Astrid Lindgren", PublishedYear = 1963 },
            new BookEntity { ISBN = TestData.Isbn13_3, Title = "Harry Potter", Author = "J.K. Rowling", PublishedYear = 1997 }
        );
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        var results = await repository.SearchAsync("Lindgren");

        // Assert
        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        using var context = CreateContext(nameof(GetAllAsync_ShouldReturnAllBooks));
        context.Books.AddRange(
            new BookEntity { ISBN = TestData.Isbn13_1, Title = "Bok 1", Author = "Författare 1", PublishedYear = 2020 },
            new BookEntity { ISBN = TestData.Isbn13_2, Title = "Bok 2", Author = "Författare 2", PublishedYear = 2021 },
            new BookEntity { ISBN = TestData.Isbn13_3, Title = "Bok 3", Author = "Författare 3", PublishedYear = 2022 }
        );
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        var results = await repository.GetAllAsync();

        // Assert
        Assert.Equal(3, results.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyBook()
    {
        // Arrange
        using var context = CreateContext(nameof(UpdateAsync_ShouldModifyBook));
        var book = new BookEntity
        {
            ISBN = TestData.Isbn13_1,
            Title = "Originaltitel",
            Author = "Författare",
            PublishedYear = 2020,
            IsAvailable = true
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        book.IsAvailable = false;
        await repository.UpdateAsync(book);

        // Assert
        var updatedBook = await context.Books.FindAsync(book.Id);
        Assert.False(updatedBook!.IsAvailable);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveBook()
    {
        // Arrange
        using var context = CreateContext(nameof(DeleteAsync_ShouldRemoveBook));
        var book = new BookEntity
        {
            ISBN = TestData.Isbn10_1,
            Title = "Ska tas bort",
            Author = "Författare",
            PublishedYear = 2020
        };
        context.Books.Add(book);
        await context.SaveChangesAsync();
        var repository = new BookRepository(context);

        // Act
        await repository.DeleteAsync(book.Id);

        // Assert
        var deletedBook = await context.Books.FindAsync(book.Id);
        Assert.Null(deletedBook);
    }
}
