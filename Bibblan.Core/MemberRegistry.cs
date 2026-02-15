namespace Bibblan.Core;

public class MemberRegistry
{
    private readonly List<Member> _members = new List<Member>();
    public IReadOnlyList<Member> Members => _members;

    public bool AddMember(Member member)
    {
        // Lägger till medlem i registret.
        if (member == null)
            throw new ArgumentNullException(nameof(member));
        _members.Add(member);
        return true;
    }

    public Member GetMemberById(string memberId)
    {
        // Hämtar medlem baserat på medlems-ID.
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Medlems-ID får inte vara tomt.", nameof(memberId));
        return _members.FirstOrDefault(m => m.MemberId == memberId);
    }

    public bool RemoveMember(string memberId)
    {
        // Tar bort medlem om den finns.
        var member = GetMemberById(memberId);
        if (member == null) return false;
        return _members.Remove(member);
    }
}
