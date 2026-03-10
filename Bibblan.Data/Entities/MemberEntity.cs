using System.ComponentModel.DataAnnotations;

namespace Bibblan.Data.Entities;

/// <summary>
/// EF-entitet för medlem med primärnyckel och navigeringsproperties.
/// </summary>
public class MemberEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string MemberId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime MemberSince { get; set; } = DateTime.Now;

    // Navigeringsproperties
    public ICollection<LoanEntity> Loans { get; set; } = new List<LoanEntity>();
    public ICollection<BookEntity> ReservedBooks { get; set; } = new List<BookEntity>();
}
