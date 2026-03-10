using System.ComponentModel.DataAnnotations;
using Bibblan.Data.Validation;

namespace Bibblan.Data.Entities;

/// <summary>
/// EF-entitet för bok med primärnyckel och navigeringsproperties.
/// </summary>
public class BookEntity
{
    public int Id { get; set; }

    [Required(ErrorMessage = "ISBN är obligatoriskt.")]
    [MaxLength(20)]
    [Isbn]
    public string ISBN { get; set; } = string.Empty;

    /// <summary>
    /// Returnerar ISBN formaterat med bindestreck för läsbarhet.
    /// </summary>
    public string FormattedISBN => IsbnHelper.Format(ISBN);

    [Required(ErrorMessage = "Titel är obligatoriskt.")]
    [MaxLength(200)]
    [MinLength(1, ErrorMessage = "Titel får inte vara tom.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Författare är obligatoriskt.")]
    [MaxLength(100)]
    [MinLength(1, ErrorMessage = "Författare får inte vara tom.")]
    public string Author { get; set; } = string.Empty;

    [Range(1450, 2100, ErrorMessage = "Utgivningsår måste vara mellan 1450 och 2100.")]
    public int PublishedYear { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsReserved { get; set; } = false;

    public int? ReservedByMemberId { get; set; }

    // Navigeringsproperties
    public MemberEntity? ReservedBy { get; set; }
    public ICollection<LoanEntity> Loans { get; set; } = new List<LoanEntity>();
}
