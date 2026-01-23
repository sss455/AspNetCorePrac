namespace SelfAspNet.Models;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<int> CreateAsync(Book book);
}