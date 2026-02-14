namespace Bibblan.Core;

public class MemberRegistry
{
    private readonly List<Member> _members = new List<Member>();
    public IReadOnlyList<Member> Members => _members;

    public bool AddMember(Member member)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        _members.Add(member);
        return true;
    }

    public Member GetMemberById(string memberId)
    {
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Medlems-ID får inte vara tomt.", nameof(memberId));
        return _members.FirstOrDefault(m => m.MemberId == memberId);
    }

    public bool RemoveMember(string memberId)
    {
        var member = GetMemberById(memberId);
        if (member == null) return false;
        return _members.Remove(member);
    }
}
