namespace Bibblan.Tests;

/// <summary>
/// Hjälpklass med konstanter för tester.
/// </summary>
public static class TestData
{
    // Giltiga ISBN-nummer för tester
    public const string Isbn13_1 = "9789100123456";  // 978-91-0-012345-6
    public const string Isbn13_2 = "9789101234567";  // 978-91-0-123456-7
    public const string Isbn13_3 = "9789102345678";  // 978-91-0-234567-8
    public const string Isbn10_1 = "9100123456";     // 91-0-012345-6
    public const string Isbn10_2 = "9101234567";     // 91-0-123456-7

    // Formaterade ISBN
    public const string Isbn13_1_Formatted = "978-91-0-012345-6";
    public const string Isbn13_2_Formatted = "978-91-0-123456-7";
    public const string Isbn10_1_Formatted = "91-0-012345-6";
}
