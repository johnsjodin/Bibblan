using Bibblan.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bibblan.Data.Repositories;

/// <summary>
/// Repository-implementation för böcker.
/// Hanterar alla databasoperationer för bokentiteter.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookEntity>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.ReservedBy)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<BookEntity?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Loans)
                .ThenInclude(l => l.Member)
            .Include(b => b.ReservedBy)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<BookEntity?> GetByISBNAsync(string isbn)
    {
        return await _context.Books
            .Include(b => b.ReservedBy)
            .FirstOrDefaultAsync(b => b.ISBN == isbn);
    }

    public async Task<BookEntity> AddAsync(BookEntity book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task UpdateAsync(BookEntity book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<BookEntity>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var term = searchTerm.ToLower();
        return await _context.Books
            .Include(b => b.ReservedBy)
            .Where(b => b.Title.ToLower().Contains(term)
                     || b.Author.ToLower().Contains(term)
                     || b.ISBN.ToLower().Contains(term))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<int> GetAvailableCountAsync()
    {
        return await _context.Books.CountAsync(b => b.IsAvailable);
    }
}
