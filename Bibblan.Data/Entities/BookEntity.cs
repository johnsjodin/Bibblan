using System.ComponentModel.DataAnnotations;

namespace Bibblan.Data.Entities;

/// <summary>
/// EF-entitet för bok med primärnyckel och navigeringsproperties.
/// </summary>
public class BookEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    public int PublishedYear { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsReserved { get; set; } = false;

    public int? ReservedByMemberId { get; set; }

    // Navigeringsproperties
    public MemberEntity? ReservedBy { get; set; }
    public ICollection<LoanEntity> Loans { get; set; } = new List<LoanEntity>();
}
