// p.475 [Add] 構成値を自作のクラスにマッピングするーーOptionsパターン
namespace SelfAspNetCore.Lib.MyOptions;

// 取得する構成データに対応するOptionsクラスを実装する
public class MyAppOptions
{
    /// <summary>書籍名</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>著者</summary>
    public string Author { get; set; } = string.Empty;
    /// <summary>刊行日</summary>
    public DateTime Published { get; set; }
    /// <summary>プロジェクト</summary>
    public List<string> Projects { get; set; } = new();
}
