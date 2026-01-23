using SelfAspNet.Models;

namespace SelfAspNet.Tests.Models;

public class BookRepositoryMock : IBookRepository
{
    public Task<int> CreateAsync(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        return Task.FromResult(new List<Book>
        {
            new()
            {
                Id = 1,
                Isbn = "978-4-7981-8094-6",
                Title = "独習Java",
                Price = 3960,
                Publisher = "翔泳社",
                Published = new DateTime(2024, 02, 15),
                Sample = true
            },
            new()
            {
                Id = 2,
                Isbn = "978-4-7981-7613-0",
                Title = "Androidアプリ開発の教科書",
                Price = 3135,
                Publisher = "翔泳社",
                Published = new DateTime(2023, 01, 24),
                Sample = true
            }
        }.AsEnumerable());
    }
}