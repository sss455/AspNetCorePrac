// p.446 [Add] 自作サービスの登録
using Microsoft.EntityFrameworkCore;

namespace Chapter07.Models.Repositories;

// [2] リポジトリクラスを定義する
// ※インターフェイスを実装したリポジトリクラス本体を作成
public class BookRepository : IBookRepository
{
    // DBコンテキスト
    private MyContext _db;

    // コンストラクター
    public BookRepository(MyContext db)
    {
        _db = db;
    }

    /// <inheritdoc/>
    public async Task<int> CreateAsync(Book book)
    {
        _db.Books.Add(book);
        return await _db.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _db.Books.ToListAsync();
    }
}

// p.477 [Add] AddXxxxx拡張メソッド
public static class BookRepositoryExtensions
{
    // ここではそこまでする意味はないが、より汎用的なサービスを登録するならば、
    // 下記のようなAddXxxxx拡張メソッドを準備しておくと便利。
    public static IServiceCollection AddBookRepository( 
                                                // 拡張メソッド
                                                this IServiceCollection services )
    {
        return services.AddTransient<IBookRepository, BookRepository>();
    }
}