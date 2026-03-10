using Bibblan.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bibblan.Data.Repositories;

/// <summary>
/// Repository-implementation för lån.
/// Hanterar alla databasoperationer för lånentiteter.
/// </summary>
public class LoanRepository : ILoanRepository
{
    private readonly LibraryContext _context;

    public LoanRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LoanEntity>> GetAllAsync()
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanEntity>> GetActiveLoansAsync()
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null)
            .OrderBy(l => l.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanEntity>> GetOverdueLoansAsync()
    {
        var now = DateTime.Now;
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null && l.DueDate < now)
            .OrderBy(l => l.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanEntity>> GetLoansByMemberIdAsync(int memberId)
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Where(l => l.MemberId == memberId)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoanEntity>> GetLoansByBookIdAsync(int bookId)
    {
        return await _context.Loans
            .Include(l => l.Member)
            .Where(l => l.BookId == bookId)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();
    }

    public async Task<LoanEntity?> GetByIdAsync(int id)
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<LoanEntity> AddAsync(LoanEntity loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return loan;
    }

    public async Task UpdateAsync(LoanEntity loan)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetActiveLoansCountAsync()
    {
        return await _context.Loans.CountAsync(l => l.ReturnDate == null);
    }

    public async Task<int> GetOverdueLoansCountAsync()
    {
        var now = DateTime.Now;
        return await _context.Loans.CountAsync(l => l.ReturnDate == null && l.DueDate < now);
    }
}
