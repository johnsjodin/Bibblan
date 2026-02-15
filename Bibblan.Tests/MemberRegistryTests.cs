using Bibblan.Core;
namespace Bibblan.Tests;

public class MemberRegistryTests
{
    [Fact]
    public void AddMember_ShouldAddMemberToRegistry()
    {
        // Arrange
        var registry = new MemberRegistry();
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");

        // Act
        var result = registry.AddMember(member);

        // Assert
        Assert.True(result);
        Assert.Single(registry.Members);
        Assert.Contains(member, registry.Members);
    }

    [Fact]
    public void AddMember_ShouldThrowException_WhenMemberIsNull()
    {
        // Arrange
        var registry = new MemberRegistry();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => registry.AddMember(null));
    }

    [Fact]
    public void AddMember_ShouldAddMultipleMembers()
    {
        // Arrange
        var registry = new MemberRegistry();
        var member1 = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var member2 = new Member("67890", "Anna Andersson", "anna@testemail.se");
        var member3 = new Member("11111", "Erik Eriksson", "erik@testemail.se");

        // Act
        registry.AddMember(member1);
        registry.AddMember(member2);
        registry.AddMember(member3);

        // Assert
        Assert.Equal(3, registry.Members.Count);
    }

    [Fact]
    public void GetMemberById_ShouldReturnMember_WhenMemberExists()
    {
        // Arrange
        var registry = new MemberRegistry();
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        registry.AddMember(member);

        // Act
        var result = registry.GetMemberById("12345");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(member, result);
    }

    [Fact]
    public void GetMemberById_ShouldReturnNull_WhenMemberDoesNotExist()
    {
        // Arrange
        var registry = new MemberRegistry();
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        registry.AddMember(member);

        // Act
        var result = registry.GetMemberById("99999");

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetMemberById_ShouldThrowException_WhenMemberIdIsInvalid(string memberId)
    {
        // Arrange
        var registry = new MemberRegistry();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => registry.GetMemberById(memberId));
    }

    [Fact]
    public void RemoveMember_ShouldReturnTrue_WhenMemberExists()
    {
        // Arrange
        var registry = new MemberRegistry();
        var member = new Member("12345", "Johan Johansson", "johan@testemail.se");
        registry.AddMember(member);

        // Act
        var result = registry.RemoveMember("12345");

        // Assert
        Assert.True(result);
        Assert.Empty(registry.Members);
    }

    [Fact]
    public void RemoveMember_ShouldReturnFalse_WhenMemberDoesNotExist()
    {
        // Arrange
        var registry = new MemberRegistry();

        // Act
        var result = registry.RemoveMember("99999");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveMember_ShouldRemoveCorrectMember_WhenMultipleMembersExist()
    {
        // Arrange
        var registry = new MemberRegistry();
        var member1 = new Member("12345", "Johan Johansson", "johan@testemail.se");
        var member2 = new Member("67890", "Anna Andersson", "anna@testemail.se");
        var member3 = new Member("11111", "Erik Eriksson", "erik@testemail.se");
        registry.AddMember(member1);
        registry.AddMember(member2);
        registry.AddMember(member3);

        // Act
        var result = registry.RemoveMember("67890");

        // Assert
        Assert.True(result);
        Assert.Equal(2, registry.Members.Count);
        Assert.Contains(member1, registry.Members);
        Assert.DoesNotContain(member2, registry.Members);
        Assert.Contains(member3, registry.Members);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyMemberList()
    {
        // Arrange & Act
        var registry = new MemberRegistry();

        // Assert
        Assert.NotNull(registry.Members);
        Assert.Empty(registry.Members);
    }
}
