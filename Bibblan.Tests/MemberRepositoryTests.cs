using Bibblan.Data;
using Bibblan.Data.Entities;
using Bibblan.Data.Repositories;
using Microsoft.EntityFrameworkCore;

#pragma warning disable xUnit1051 // CancellationToken i testmetoder är inte kritiskt

namespace Bibblan.Tests;

/// <summary>
/// Tester för MemberRepository och automatisk medlemsnummergenerering.
/// </summary>
public class MemberRepositoryTests
{
    private LibraryContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new LibraryContext(options);
    }

    [Fact]
    public async Task GenerateNextMemberIdAsync_WhenNoMembers_ReturnsM001()
    {
        // Arrange
        using var context = CreateContext(nameof(GenerateNextMemberIdAsync_WhenNoMembers_ReturnsM001));
        var repository = new MemberRepository(context);

        // Act
        var nextId = await repository.GenerateNextMemberIdAsync();

        // Assert
        Assert.Equal("M001", nextId);
    }

    [Fact]
    public async Task GenerateNextMemberIdAsync_WhenM005Exists_ReturnsM006()
    {
        // Arrange
        using var context = CreateContext(nameof(GenerateNextMemberIdAsync_WhenM005Exists_ReturnsM006));
        context.Members.AddRange(
            new MemberEntity { MemberId = "M001", Name = "Test 1", Email = "test1@test.se" },
            new MemberEntity { MemberId = "M003", Name = "Test 3", Email = "test3@test.se" },
            new MemberEntity { MemberId = "M005", Name = "Test 5", Email = "test5@test.se" }
        );
        await context.SaveChangesAsync();

        var repository = new MemberRepository(context);

        // Act
        var nextId = await repository.GenerateNextMemberIdAsync();

        // Assert
        Assert.Equal("M006", nextId);
    }

    [Fact]
    public async Task GenerateNextMemberIdAsync_WithGaps_ReturnsNextAfterHighest()
    {
        // Arrange - Luckor i nummerserien (M001, M010)
        using var context = CreateContext(nameof(GenerateNextMemberIdAsync_WithGaps_ReturnsNextAfterHighest));
        context.Members.AddRange(
            new MemberEntity { MemberId = "M001", Name = "Test 1", Email = "test1@test.se" },
            new MemberEntity { MemberId = "M010", Name = "Test 10", Email = "test10@test.se" }
        );
        await context.SaveChangesAsync();

        var repository = new MemberRepository(context);

        // Act
        var nextId = await repository.GenerateNextMemberIdAsync();

        // Assert - Ska returnera M011 (efter högsta), inte fylla luckor
        Assert.Equal("M011", nextId);
    }

    [Fact]
    public async Task AddAsync_ShouldCreateMember()
    {
        // Arrange
        using var context = CreateContext(nameof(AddAsync_ShouldCreateMember));
        var repository = new MemberRepository(context);
        var member = new MemberEntity { MemberId = "M001", Name = "Anna Andersson", Email = "anna@test.se" };

        // Act
        await repository.AddAsync(member);

        // Assert
        var savedMember = await context.Members.FirstOrDefaultAsync();
        Assert.NotNull(savedMember);
        Assert.Equal("M001", savedMember.MemberId);
        Assert.Equal("Anna Andersson", savedMember.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectMember()
    {
        // Arrange
        using var context = CreateContext(nameof(GetByIdAsync_ShouldReturnCorrectMember));
        var member = new MemberEntity { MemberId = "M001", Name = "Test", Email = "test@test.se" };
        context.Members.Add(member);
        await context.SaveChangesAsync();

        var repository = new MemberRepository(context);

        // Act
        var result = await repository.GetByIdAsync(member.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("M001", result.MemberId);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMembers()
    {
        // Arrange
        using var context = CreateContext(nameof(GetAllAsync_ShouldReturnAllMembers));
        context.Members.AddRange(
            new MemberEntity { MemberId = "M001", Name = "Test 1", Email = "test1@test.se" },
            new MemberEntity { MemberId = "M002", Name = "Test 2", Email = "test2@test.se" }
        );
        await context.SaveChangesAsync();

        var repository = new MemberRepository(context);

        // Act
        var members = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, members.Count());
    }
}
