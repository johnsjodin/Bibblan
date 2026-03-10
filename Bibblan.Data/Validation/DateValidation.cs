using System.ComponentModel.DataAnnotations;

namespace Bibblan.Data.Validation;

/// <summary>
/// Hjälpklass för datumvalidering.
/// </summary>
public static class DateHelper
{
    /// <summary>
    /// Validerar att ett utgivningsår är inom tillåtet intervall.
    /// </summary>
    public static bool IsValidPublishedYear(int year, int minYear = 1450)
    {
        var currentYear = DateTime.Now.Year;
        return year >= minYear && year <= currentYear;
    }

    /// <summary>
    /// Validerar att ett datum inte är i framtiden.
    /// </summary>
    public static bool IsNotInFuture(DateTime date)
    {
        return date.Date <= DateTime.Now.Date;
    }
}

/// <summary>
/// Valideringsattribut för publiceringsår.
/// Tillåter inte år i framtiden.
/// </summary>
public class PublishedYearAttribute : ValidationAttribute
{
    private readonly int _minYear;

    public PublishedYearAttribute(int minYear = 1450)
    {
        _minYear = minYear;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int year)
            return new ValidationResult("Utgivningsår måste anges.");

        if (!DateHelper.IsValidPublishedYear(year, _minYear))
        {
            var currentYear = DateTime.Now.Year;
            if (year < _minYear)
                return new ValidationResult($"Utgivningsår kan inte vara före {_minYear}.");
            if (year > currentYear)
                return new ValidationResult($"Utgivningsår kan inte vara i framtiden (max {currentYear}).");
        }

        return ValidationResult.Success;
    }
}

/// <summary>
/// Valideringsattribut för lånedatum.
/// Tillåter inte datum i framtiden.
/// </summary>
public class NotInFutureAttribute : ValidationAttribute
{
    public NotInFutureAttribute()
    {
        ErrorMessage = "Datumet kan inte vara i framtiden.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date && !DateHelper.IsNotInFuture(date))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

/// <summary>
/// Valideringsattribut för förfallodatum.
/// Validerar att datumet är efter ett annat datumfält.
/// </summary>
public class MustBeAfterAttribute : ValidationAttribute
{
    private readonly string _comparisonProperty;

    public MustBeAfterAttribute(string comparisonProperty)
    {
        _comparisonProperty = comparisonProperty;
        ErrorMessage = $"Datumet måste vara efter {comparisonProperty}.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime endDate)
            return ValidationResult.Success;

        var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
        if (property == null)
            return new ValidationResult($"Egenskapen {_comparisonProperty} hittades inte.");

        var startDate = property.GetValue(validationContext.ObjectInstance) as DateTime?;
        if (startDate == null)
            return ValidationResult.Success;

        if (endDate <= startDate)
            return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}
