using Microsoft.EntityFrameworkCore;

namespace SelfAspNet.Models;

public class BookRepository : IBookRepository
{
    private readonly MyContext _db;
    public BookRepository(MyContext db)
    {
        _db = db;
    }
    public async Task<int> CreateAsync(Book book)
    {
        _db.Books.Add(book);
        return await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _db.Books.ToListAsync();
    }

}

public static class BookRepositoryExtensions
{
    public static IServiceCollection AddBookRepository(
        this IServiceCollection services)
    {
        return services.AddTransient<IBookRepository, BookRepository>();
    }
}