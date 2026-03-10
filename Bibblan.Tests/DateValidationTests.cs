using System.ComponentModel.DataAnnotations;
using Bibblan.Data.Validation;

namespace Bibblan.Tests;

/// <summary>
/// Tester för datumvalideringsattribut.
/// </summary>
public class DateValidationTests
{
    #region PublishedYearAttribute Tests

    [Fact]
    public void PublishedYear_ShouldAcceptCurrentYear()
    {
        // Arrange
        var attribute = new PublishedYearAttribute();
        var currentYear = DateTime.Now.Year;

        // Act
        var result = attribute.GetValidationResult(currentYear, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void PublishedYear_ShouldAcceptPastYear()
    {
        // Arrange
        var attribute = new PublishedYearAttribute();

        // Act
        var result = attribute.GetValidationResult(2000, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void PublishedYear_ShouldRejectFutureYear()
    {
        // Arrange
        var attribute = new PublishedYearAttribute();
        var futureYear = DateTime.Now.Year + 1;

        // Act
        var result = attribute.GetValidationResult(futureYear, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("framtiden", result!.ErrorMessage);
    }

    [Fact]
    public void PublishedYear_ShouldRejectYearBeforeMinimum()
    {
        // Arrange
        var attribute = new PublishedYearAttribute(1450);

        // Act
        var result = attribute.GetValidationResult(1400, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("1450", result!.ErrorMessage);
    }

    [Theory]
    [InlineData(1450)]  // Minsta tillåtna
    [InlineData(1500)]
    [InlineData(1945)]
    [InlineData(2020)]
    public void PublishedYear_ShouldAcceptValidYears(int year)
    {
        // Arrange
        var attribute = new PublishedYearAttribute();

        // Act
        var result = attribute.GetValidationResult(year, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region NotInFutureAttribute Tests

    [Fact]
    public void NotInFuture_ShouldAcceptToday()
    {
        // Arrange
        var attribute = new NotInFutureAttribute();

        // Act
        var result = attribute.GetValidationResult(DateTime.Now.Date, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void NotInFuture_ShouldAcceptPastDate()
    {
        // Arrange
        var attribute = new NotInFutureAttribute();
        var pastDate = DateTime.Now.AddDays(-7);

        // Act
        var result = attribute.GetValidationResult(pastDate, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void NotInFuture_ShouldRejectFutureDate()
    {
        // Arrange
        var attribute = new NotInFutureAttribute();
        var futureDate = DateTime.Now.AddDays(1);

        // Act
        var result = attribute.GetValidationResult(futureDate, new ValidationContext(new object()));

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
        Assert.Contains("framtiden", result!.ErrorMessage);
    }

    [Fact]
    public void NotInFuture_ShouldAcceptNullValue()
    {
        // Arrange
        var attribute = new NotInFutureAttribute();

        // Act
        var result = attribute.GetValidationResult(null, new ValidationContext(new object()));

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }

    #endregion

    #region DateHelper Tests

    [Theory]
    [InlineData(1450, true)]   // Minsta tillåtna
    [InlineData(1500, true)]
    [InlineData(2000, true)]
    [InlineData(1449, false)]  // Före minimum
    public void DateHelper_IsValidPublishedYear_ValidatesCorrectly(int year, bool expected)
    {
        // Act
        var result = DateHelper.IsValidPublishedYear(year);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DateHelper_IsValidPublishedYear_RejectsFutureYear()
    {
        // Arrange
        var futureYear = DateTime.Now.Year + 1;

        // Act
        var result = DateHelper.IsValidPublishedYear(futureYear);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DateHelper_IsValidPublishedYear_AcceptsCurrentYear()
    {
        // Arrange
        var currentYear = DateTime.Now.Year;

        // Act
        var result = DateHelper.IsValidPublishedYear(currentYear);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateHelper_IsNotInFuture_AcceptsToday()
    {
        // Act
        var result = DateHelper.IsNotInFuture(DateTime.Now.Date);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateHelper_IsNotInFuture_AcceptsPastDate()
    {
        // Act
        var result = DateHelper.IsNotInFuture(DateTime.Now.AddDays(-30));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DateHelper_IsNotInFuture_RejectsFutureDate()
    {
        // Act
        var result = DateHelper.IsNotInFuture(DateTime.Now.AddDays(1));

        // Assert
        Assert.False(result);
    }

    #endregion
}
