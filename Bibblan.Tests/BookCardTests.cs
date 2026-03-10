using Bibblan.Data.Entities;
using Bibblan.Web.Components.Shared;
using Bunit;

namespace Bibblan.Tests;

/// <summary>
/// bUnit-tester för BookCard Blazor-komponenten.
/// Verifierar att komponentens rendering och statusvisning fungerar korrekt.
/// </summary>
public class BookCardTests : Bunit.TestContext
{
    [Fact]
    public void BookCard_ShouldDisplayBookTitle()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Pippi Långstrump",
            Author = "Astrid Lindgren",
            PublishedYear = 1945,
            ISBN = "978-91-0-012345-6",
            IsAvailable = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        cut.Find("h5.card-title").TextContent.MarkupMatches("Pippi Långstrump");
    }

    [Fact]
    public void BookCard_ShouldDisplayAuthorAndYear()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "En bok",
            Author = "Testförfattare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var authorText = cut.Find(".card-text.text-muted");
        Assert.Contains("Testförfattare", authorText.TextContent);
        Assert.Contains("2020", authorText.TextContent);
    }

    [Fact]
    public void BookCard_ShouldShowAvailableStatus_WhenBookIsAvailable()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Tillgänglig bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = true,
            IsReserved = false
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var badge = cut.Find(".badge");
        Assert.Contains("bg-success", badge.ClassName);
        Assert.Equal("Tillgänglig", badge.TextContent);
    }

    [Fact]
    public void BookCard_ShouldShowBorrowedStatus_WhenBookIsNotAvailable()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Utlånad bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = false,
            IsReserved = false
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var badge = cut.Find(".badge");
        Assert.Contains("bg-secondary", badge.ClassName);
        Assert.Equal("Utlånad", badge.TextContent);
    }

    [Fact]
    public void BookCard_ShouldShowReservedStatus_WhenBookIsReserved()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Reserverad bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = true,
            IsReserved = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var badge = cut.Find(".badge");
        Assert.Contains("bg-warning", badge.ClassName);
        Assert.Equal("Reserverad", badge.TextContent);
    }

    [Fact]
    public void BookCard_ShouldShowBorrowedAndReservedStatus_WhenBothFlagsSet()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Utlånad och reserverad",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = false,
            IsReserved = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var badge = cut.Find(".badge");
        Assert.Contains("bg-danger", badge.ClassName);
        Assert.Equal("Utlånad (Reserverad)", badge.TextContent);
    }

    [Fact]
    public void BookCard_ShouldHaveAvailableClass_WhenBookIsAvailable()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var card = cut.Find(".card.book-card");
        Assert.Contains("available", card.ClassName);
    }

    [Fact]
    public void BookCard_ShouldHaveBorrowedClass_WhenBookIsNotAvailable()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = false
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var card = cut.Find(".card.book-card");
        Assert.Contains("borrowed", card.ClassName);
    }

    [Fact]
    public void BookCard_ShouldShowActionsButton_ByDefault()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 42,
            Title = "Bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var footer = cut.Find(".card-footer");
        var link = cut.Find("a.btn");
        Assert.Contains("books/42", link.GetAttribute("href"));
    }

    [Fact]
    public void BookCard_ShouldHideActionsButton_WhenShowActionsIsFalse()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "123",
            IsAvailable = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters
                .Add(p => p.Book, book)
                .Add(p => p.ShowActions, false));

        // Assert - card-footer ska inte finnas
        var hasFooter = cut.FindAll(".card-footer").Count > 0;
        Assert.False(hasFooter, "Card-footer ska inte visas när ShowActions är false");
    }

    [Fact]
    public void BookCard_ShouldDisplayISBN()
    {
        // Arrange
        var book = new BookEntity
        {
            Id = 1,
            Title = "Bok",
            Author = "Författare",
            PublishedYear = 2020,
            ISBN = "978-91-0-012345-6",
            IsAvailable = true
        };

        // Act
        var cut = Render<BookCard>(parameters =>
            parameters.Add(p => p.Book, book));

        // Assert
        var codeElement = cut.Find("code");
        Assert.Contains("978-91-0-012345-6", codeElement.TextContent);
    }
}
