namespace Bibblan.Core;

public class MemberRegistry
{
    public List<Member> Members { get; }

    public MemberRegistry()
    {
        Members = new List<Member>();
    }

    public bool AddMember(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        Members.Add(member);
        return true;
    }

    public Member GetMemberById(string memberId)
    {
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Medlems-ID får inte vara tomt.", nameof(memberId));
        return Members.FirstOrDefault(m => m.MemberId == memberId);
    }

    public bool RemoveMember(string memberId)
    {
        var member = GetMemberById(memberId);
        if (member == null) return false;
        return Members.Remove(member);
    }
}
