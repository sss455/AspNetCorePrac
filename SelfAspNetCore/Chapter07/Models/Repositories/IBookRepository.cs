// p.446 [Add] 自作サービスの登録
namespace Chapter07.Models.Repositories;

// [1] インターフェイスを準備する
// ※リポジトリクラスはインターフェイス経由でアクセスするのが基本
public interface IBookRepository
{
    /// <summary>
    /// 書籍データを全件取得
    /// </summary>
    /// <returns>取得した書籍データ</returns>
    Task<IEnumerable<Book>> GetAllAsync();

    /// <summary>
    /// 書籍データを新規作成
    /// </summary>
    /// <param name="book">Bookエンティティ</param>
    /// <returns>作成件数</returns>
    Task<int> CreateAsync(Book book);
}
