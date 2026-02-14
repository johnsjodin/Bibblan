using System;
using System.Collections.Generic;
using System.Text;

namespace Bibblan.Core;

public class Member : ISearchable
{
    public string MemberId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime MemberSince { get; private set; }
    public List<Loan> Loans { get; private set; }

    public Member(string memberId, string name, string email)
    {
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
        Loans = new List<Loan>();
    }

    public string GetInfo()
    {
        StringBuilder loansInfo = new StringBuilder();

        if (Loans.Count == 0)
        {
            loansInfo.Append("Inga lånade böcker.");
        }
        else
        {
            foreach (Loan loan in Loans)
            {
                loansInfo.AppendLine($"  - \"{loan.Book.Title}\" av {loan.Book.Author} (Återlämnas: {loan.DueDate.ToShortDateString()})");
            }
        }

        return $"Medlem: {Name} (ID: {MemberId}, E-post: {Email}, Medlem sedan: {MemberSince.ToShortDateString()}" +
            $"\nLånade böcker:\n{loansInfo}";
    }

    public bool Matches(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return false;

        searchTerm = searchTerm.ToLower();

        return Name.ToLower().Contains(searchTerm)
            || Email.ToLower().Contains(searchTerm)
            || MemberId.ToLower().Contains(searchTerm);
    }
}
