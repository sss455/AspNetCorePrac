namespace Chapter07.Lib;


// インターフェイス：Guid値を返すIdプロパティを定義
public interface IScopedService
{
    Guid Id { get; }
}


// インスタンス時に生成されたGuid値を返すだけのサービス
public class ScopedService : IScopedService
{
    private readonly Guid _id;

    // インスタンス時に一意のGuid値を生成
    public ScopedService()
    {
        _id = Guid.NewGuid();
    }

    // インスタンスに保持したGuid値を返す
    public Guid Id => _id;
}