using Bibblan.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bibblan.Data.Repositories;

/// <summary>
/// Repository-implementation för medlemmar.
/// Hanterar alla databasoperationer för medlemsentiteter.
/// </summary>
public class MemberRepository : IMemberRepository
{
    private readonly LibraryContext _context;

    public MemberRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MemberEntity>> GetAllAsync()
    {
        return await _context.Members
            .Include(m => m.Loans.Where(l => l.ReturnDate == null))
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<MemberEntity?> GetByIdAsync(int id)
    {
        return await _context.Members
            .Include(m => m.Loans)
                .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MemberEntity?> GetByMemberIdAsync(string memberId)
    {
        return await _context.Members
            .Include(m => m.Loans)
            .FirstOrDefaultAsync(m => m.MemberId == memberId);
    }

    public async Task<MemberEntity> AddAsync(MemberEntity member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task UpdateAsync(MemberEntity member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member != null)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<MemberEntity>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var term = searchTerm.ToLower();
        return await _context.Members
            .Include(m => m.Loans.Where(l => l.ReturnDate == null))
            .Where(m => m.Name.ToLower().Contains(term)
                     || m.Email.ToLower().Contains(term)
                     || m.MemberId.ToLower().Contains(term))
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Members.CountAsync();
    }
}
