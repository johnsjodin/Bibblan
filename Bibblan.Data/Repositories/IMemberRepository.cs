using Bibblan.Data.Entities;

namespace Bibblan.Data.Repositories;

/// <summary>
/// Interface för medlem-repository.
/// Definierar dataåtkomstoperationer för medlemmar.
/// </summary>
public interface IMemberRepository
{
    Task<IEnumerable<MemberEntity>> GetAllAsync();
    Task<MemberEntity?> GetByIdAsync(int id);
    Task<MemberEntity?> GetByMemberIdAsync(string memberId);
    Task<MemberEntity> AddAsync(MemberEntity member);
    Task UpdateAsync(MemberEntity member);
    Task DeleteAsync(int id);
    Task<IEnumerable<MemberEntity>> SearchAsync(string searchTerm);
    Task<int> GetTotalCountAsync();

    /// <summary>
    /// Genererar nästa lediga medlemsnummer (M001, M002, etc.)
    /// </summary>
    Task<string> GenerateNextMemberIdAsync();
}
