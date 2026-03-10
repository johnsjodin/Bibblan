using System.ComponentModel.DataAnnotations;

namespace Bibblan.Data.Entities;

/// <summary>
/// EF-entitet för lån med relationer till bok och medlem.
/// </summary>
public class LoanEntity
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public int MemberId { get; set; }

    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // Beräknade egenskaper
    public bool IsOverdue => ReturnDate == null && DateTime.Now > DueDate;
    public bool IsReturned => ReturnDate != null;

    // Navigeringsproperties
    public BookEntity Book { get; set; } = null!;
    public MemberEntity Member { get; set; } = null!;
}
