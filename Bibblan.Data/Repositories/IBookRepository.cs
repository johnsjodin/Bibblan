using Bibblan.Data.Entities;

namespace Bibblan.Data.Repositories;

/// <summary>
/// Interface för bok-repository.
/// Definierar dataåtkomstoperationer för böcker.
/// </summary>
public interface IBookRepository
{
    Task<IEnumerable<BookEntity>> GetAllAsync();
    Task<BookEntity?> GetByIdAsync(int id);
    Task<BookEntity?> GetByISBNAsync(string isbn);
    Task<BookEntity> AddAsync(BookEntity book);
    Task UpdateAsync(BookEntity book);
    Task DeleteAsync(int id);
    Task<IEnumerable<BookEntity>> SearchAsync(string searchTerm);
    Task<int> GetTotalCountAsync();
    Task<int> GetAvailableCountAsync();
}
