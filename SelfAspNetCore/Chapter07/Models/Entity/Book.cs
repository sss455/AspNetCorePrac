using System.ComponentModel.DataAnnotations;

namespace Chapter07.Models;

// 書籍エンティティ
public class Book
{
    /// <summary>書籍ID</summary>
    public int Id { get; set; }


    /// <summary>ISBN</summary>
    // p.129 [Add] Html.DisplarForメソッド対応の自作テンプレートに適用されるように、IsbnプロパティにImageUrl型を割り当てる
    [DataType(DataType.ImageUrl)]
    // p.308 [Add] 標準的な検証機能の実装（正規表現検証）
    [RegularExpression("978-4-[0-9]{2,5}-[0-9]{2,5}-[0-9X]", ErrorMessage="{0}の形式が誤っています。")]
    // p.320 [Add] サーバーサイドと連携した検証を実装するーー Remote検証
    //  ・第1引数：検証アクション名
    //  ・第2引数：検証アクションが属するコントローラー名
    //[Remote("UniqueIsbn", "Books")]
    // Titleプロパティも送信したい場合、AddtionalFieldsプロパティを使用して以下のように記述。
    //[Remote("UniqueIsbn", "Books", AdditionalFields=nameof(Title))]  // 編集画面でも重複エラーが発生してしまうためコメントアウト

    //[UIHint("Isbn")]
    [Display(Name = "ISBN")]
    public String Isbn { get; set; } = String.Empty;


    /// <summary>タイトル</summary>
    // p.308 [Add] 標準的な検証機能の実装（必須検証、文字列長検証）
    [Required(ErrorMessage="{0}は必須です。")]
    [StringLength(50, ErrorMessage="{0}は{1}文字以内で指定してください。")]
    [Display(Name = "タイトル")]
    public String Title { get; set; } = String.Empty;


    /// <summary>価格</summary>
    // p.308 [Add] 標準的な検証機能の実装（範囲検証(数値型の範囲を判定)）
    [Range(10, 10000, ErrorMessage="{0}は{1}～{2}の間で指定してください。")]
    // p.127 [Add] DataType属性：特殊なデータ型を割り当てる（DisplayForビューヘルパー）
    // p.136 [Add] Html.EditorForメソッド対応の自作テンプレートに適用されるように、PriceプロパティにCurrency型を割り当てる
    [DataType(DataType.Currency)]
    [Display(Name = "価格")]
    public int Price { get; set; }


    /// <summary>出版社</summary>
    // p.308 [Add] 標準的な検証機能の実装（正規表現検証）
    [RegularExpression("翔泳社|技術評論社|SBクリエイティブ|日経BP|森北出版", ErrorMessage="{0}は、{1}のいずれかでなければなりません。")]
    // p.326 [Add] 検証属性の自作（サーバーサイドの実装）
    // [InOptions("翔泳社, 技術評論社, SBクリエイティブ, 日経BP, 森北出版")]
    [Display(Name = "出版社")]
    public String Publisher { get; set; } = String.Empty;


    /// <summary>刊行日</summary>
    // p.308 [Add] 標準的な検証機能の実装（範囲検証(DateTime型の範囲を判定)）
    //[Range(typeof(DateTime), "2010-01-01","2029-12-31", ErrorMessage="{0}は{1:d}～{2:d}の範囲で指定してください。")]
    // p.133 [Add] DataType属性：特殊なデータ型を割り当てる（Inputタグヘルパー）
    [DataType(DataType.Date)]
    [Display(Name = "刊行日")]
    public DateTime Published { get; set; }


    /// <summary>配布サンプル</summary>
    [Display(Name = "配布サンプル")]
    public bool Sample { get; set; }


    /// <summary>バージョン管理列</summary>
    // p.297 [Add] オプティミスティック(楽観的)同時実行制御
    // ※バージョン管理列は、byte[]型のプロパティにTimestamp属性を付与するのがルール
    [Timestamp]
    public byte[]? RowVewsion { get; set; }



    // ナビゲーションプロパティ：テーブル同士の関係（リレーションシップ）をエンティティの世界で表現する仕組み。
    public ICollection<Review> Reviews { get; } = new List<Review>();
    //public ICollection<Review> Reviews2 = []; // ← C#12であればコレクション式を使って、単に[]とあらわしてもOK
    //-------------------------------------------------------------------------------------
    //【補足】
    //   Book:Reviews(1:n)の関係を表すナビゲーションプロパティは、以下のようなルールで構成する。
    //    ・名前は複数形（ここではReviews）
    //    ・戻り値はICollection<T>型
    //    ・ゲッター(get)のみで、規定値は空のリスト
    //-------------------------------------------------------------------------------------

    // ナビゲーションプロパティ（リレーションシップ）
    public ICollection<Author> Authors { get; } = new List<Author>();


    // // p.219 [Add]「ナビゲーションプロパティへのアクセス(遅延読み込み)」用にvirtual修飾子を付与。
    // // ナビゲーションプロパティ（リレーションシップ）
    // public virtual ICollection<Review> Reviews { get; } = new List<Review>();
    // public virtual ICollection<Author> Authors { get; } = new List<Author>();
}
