using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Bibblan.Core;

public class Member : ISearchable
{
    public string MemberId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime MemberSince { get; private set; }

    private readonly List<Loan> _loans = new List<Loan>();
    public IReadOnlyList<Loan> Loans => _loans;

    public Member(string memberId, string name, string email)
    {
        // Validerar och skapar en ny medlem.
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Medlems-ID får inte vara tomt.", nameof(memberId));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Namn får inte vara tomt.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-post får inte vara tomt.", nameof(email));
        MemberId = memberId;
        Name = name;
        Email = email;
        MemberSince = DateTime.Now;
    }

    internal void AddLoan(Loan loan)
    {
        // Lägger till ett lån kopplat till medlemmen.
        if (loan == null)
            throw new ArgumentNullException(nameof(loan));
        _loans.Add(loan);
    }

    public string GetInfo()
    {
        // Bygger en översikt av medlem och aktiva lån.
        StringBuilder loansInfo = new StringBuilder();
        var activeLoans = Loans.Where(l => !l.IsReturned).ToList();

        if (activeLoans.Count == 0)
        {
            loansInfo.Append("Inga lånade böcker.");
        }
        else
        {
            foreach (Loan loan in activeLoans)
            {
                loansInfo.AppendLine($"  - \"{loan.Book.Title}\" av {loan.Book.Author} (Återlämnas: {loan.DueDate.ToShortDateString()})");
            }
        }

        return $"Medlem: {Name} (ID: {MemberId}, E-post: {Email}, Medlem sedan: {MemberSince.ToShortDateString()})" +
            $"\nLånade böcker:\n{loansInfo}";
    }

    public bool Matches(string searchTerm)
    {
        // Matchar sökterm mot medlemsuppgifter.
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        return Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            || MemberId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);
    }
}
