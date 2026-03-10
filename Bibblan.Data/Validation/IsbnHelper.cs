using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bibblan.Data.Validation;

/// <summary>
/// Valideringsattribut för ISBN-koder (ISBN-10 eller ISBN-13).
/// </summary>
public partial class IsbnAttribute : ValidationAttribute
{
    public IsbnAttribute() : base("ISBN måste vara 10 eller 13 siffror.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string isbn || string.IsNullOrWhiteSpace(isbn))
            return new ValidationResult("ISBN måste anges.");

        if (!IsbnHelper.IsValid(isbn))
            return new ValidationResult("ISBN måste vara exakt 10 eller 13 siffror.");

        return ValidationResult.Success;
    }
}

/// <summary>
/// Hjälpklass för ISBN-hantering: validering och formatering.
/// </summary>
public static partial class IsbnHelper
{
    /// <summary>
    /// Extraherar endast siffror från ISBN-sträng.
    /// </summary>
    public static string GetDigitsOnly(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return string.Empty;

        return DigitsOnlyRegex().Replace(isbn, "");
    }

    /// <summary>
    /// Validerar om ISBN är giltigt (10 eller 13 siffror).
    /// </summary>
    public static bool IsValid(string isbn)
    {
        var digits = GetDigitsOnly(isbn);
        return digits.Length == 10 || digits.Length == 13;
    }

    /// <summary>
    /// Formaterar ISBN med bindestreck för bättre läsbarhet.
    /// ISBN-13: 978-91-0-012345-6
    /// ISBN-10: 91-0-012345-6
    /// </summary>
    public static string Format(string isbn)
    {
        var digits = GetDigitsOnly(isbn);

        return digits.Length switch
        {
            13 => FormatIsbn13(digits),
            10 => FormatIsbn10(digits),
            _ => isbn // Returnera original om ogiltigt
        };
    }

    /// <summary>
    /// Formaterar ISBN-13: 978-XX-X-XXXXXX-X
    /// </summary>
    private static string FormatIsbn13(string digits)
    {
        // Format: 978-91-0-012345-6 (prefix-grupp-utgivare-titel-kontroll)
        return $"{digits[..3]}-{digits[3..5]}-{digits[5]}-{digits[6..12]}-{digits[12]}";
    }

    /// <summary>
    /// Formaterar ISBN-10: XX-X-XXXXXX-X
    /// </summary>
    private static string FormatIsbn10(string digits)
    {
        // Format: 91-0-012345-6 (grupp-utgivare-titel-kontroll)
        return $"{digits[..2]}-{digits[2]}-{digits[3..9]}-{digits[9]}";
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex DigitsOnlyRegex();
}
