using Bibblan.Data.Entities;

namespace Bibblan.Data.Repositories;

/// <summary>
/// Interface för lån-repository.
/// Definierar dataåtkomstoperationer för lån.
/// </summary>
public interface ILoanRepository
{
    Task<IEnumerable<LoanEntity>> GetAllAsync();
    Task<IEnumerable<LoanEntity>> GetActiveLoansAsync();
    Task<IEnumerable<LoanEntity>> GetOverdueLoansAsync();
    Task<IEnumerable<LoanEntity>> GetLoansByMemberIdAsync(int memberId);
    Task<IEnumerable<LoanEntity>> GetLoansByBookIdAsync(int bookId);
    Task<LoanEntity?> GetByIdAsync(int id);
    Task<LoanEntity> AddAsync(LoanEntity loan);
    Task UpdateAsync(LoanEntity loan);
    Task<int> GetActiveLoansCountAsync();
    Task<int> GetOverdueLoansCountAsync();
}
