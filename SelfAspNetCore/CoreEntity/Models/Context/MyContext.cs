
using System.Net.Mail;
using CoreEntity.Lib.CustomTypeConverter;
using CoreEntity.Models.CustomType;
using CoreEntity.Models.EntityTypeConfiguration;
using CoreEntity.Models.MyEnum;
using CoreEntity.Models.ViewEntity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreEntity.Models;

// コンテキストクラス：エンティティをデータベースに橋渡しする
// ※コンテキストであることの条件は、以下の(1)～(3)のとおり。（TContextはコンテキスト型、TModelはエンティティ型を表す）
public class MyContext : DbContext // (1) DbContextクラスを継承すること
{
    // (2) DbContextOptions<TContext>型のコンストラクターを定義すること
    public MyContext (DbContextOptions<MyContext> options) : base (options) { }

    // (3) DbSet<Tmodel>型のpublicプロパティを持つこと（名前はエンティティの複数形に対応）
    public DbSet<Book> Books { get; set; }
    //→ MyContext#Booksプロパティで、同名のBooksテーブルにアクセスし、すべてのレコードを取得できるようになる。
    //  複数のテーブルを扱うならば、同様のプロパティを列挙する。


    // レビュー情報テーブル
    public DbSet<Review> Reviews { get; set; }
    // ユーザー情報テーブル
    public DbSet<User> Users { get; set; }
    // 会社情報テーブル
    public DbSet<Company> Companies { get; set; }
    // 従業員情報テーブル
    public DbSet<Employee> Employees { get; set; }
    // 記事情報テーブル
    public DbSet<Article> Articles { get; set; }
    // タイアップ記事情報テーブル
    public DbSet<CollabArticle> CollabArticles { get; set; }

    // p.251 [Add] マイグレーションでビューを定義する（Sqlメソッド）
    // ビューのDbSetを定義
    public DbSet<ViewPubCount> ViewPubCounts { get; set; }


    // p.233 [Add] Fluent API の基本
    // Fluent APIではコンテキストの「OnModelCreatingメソッド」をオーバーライドすることでモデルを定義するのが基本
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // //「ModelBuilder#Entryメソッド」で目的のエンティティを取得
        // // ⇒配下のラムダ式でエンティティ／プロパティのマッピングを記述していく
        // //                           review：EntityTypeBuilder<T>型
        // modelBuilder.Entity<Review>( review =>
        //     {
        //         review.ToTable("Comments")          // テーブルを設定（ReviewエンティティをCommentsテーブルに紐づけ）
        //                 .HasKey(r => r.Code);       // 主キーを設定 （Codeプロパティ）

        //         review.Property(r => r.Body)        // プロパティを取得（Bodyプロパティ）
        //                 .HasColumnName("Message")   // 列名を設定（Bodyプロパティに対応する列名をMessageに）
        //                 .HasMaxLength(150);         // 最大長を設定（Bodyプロパティの最大長を150文字に）
        //     }
        // );
        // ⇒ ※モデル定義をエンティティ単位に分割（ReviewEntiryTypeConfiguration.csへ移動）


        // p.236 [Add] エンティティ単位(Review)に分割したモデル定義を呼び出す
        new ReviewEntityTypeConfiguration().Configure(
            modelBuilder.Entity<Review>()
        );


        // p.244 [Add] データベースの値を暗黙的に型変換する（値コンバーター）
        // ※列挙型を登録する際に、文字列として変換する例
        modelBuilder.Entity<User>(e =>
            {
                e.Property(e => e.UserClass)
                .HasConversion(
                    v => v.ToString(),                                // 書き込み時に変換するための値の型（列挙型⇒文字列）
                    v => (UserClass) Enum.Parse(typeof(UserClass), v) // 読み込み時に変換するための値の型（文字列⇒列挙型）
                );
            }
        );


        // // p.242 [Add] 特定の型全体に対してコンバーターを適用する（値コンバーター）
        // // 【別解】ValueConverterを直接にインスタンスすることもできる。
        // //        別ファイルとして切り出すほどではない、簡単なコンバーターを定義する際に利用
        // modelBuilder.Entity<User>(e => {
        //         e.Property(e => e.Email)
        //         .HasConversion( new 
        //             ValueConverter<EmailAddress?, string> (
        //                 v => v==null ? "" : v.ToString(), // 書き込み時：EmailAddress#ToStoringメソッドで文字列化
        //                 v => new EmailAddress(v)          // 読み込み時：EmailAddressオブジェクトをインスタンス化
        //             )
        //         );
        //     }
        // );
        // ⇒ 別ファイルに切り出す例としては「Lib/CustomTypeConverter/EmailAddressConverter.cs」に切り出し


        // p.251 [Add] マイグレーションでビューを定義する（Sqlメソッド）
        // Fluent APiでToViewメソッドを呼び出し、ViewPubCountエンティティが、ViewPubCountsビューに紐づくことを宣言
        modelBuilder.Entity<ViewPubCount>()
                    .ToView("ViewPubCounts");
    }


    // p.242 [Add] 特定の型全体に対してコンバーターを適用する（値コンバーター）
    // EmailAddress型を準備し、メールアドレスをアプリで扱う場合にはEmailAddress型として、データベースに格納する際にはVARCHAR型として扱う。
    //
    // ConfigureConventions：エンティティに関わる規約（型マッピング、変換など）を定義するためのメソッド
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // EmailAddress型の値コンバーターを登録
        configurationBuilder
            // 指定型のプロパティを設定するためのビルダーを取得
            .Properties<EmailAddress>()
            // 型コンバーターに従って型を変換
            .HaveConversion<EmailAddressConverter>();
    }



    // // 著者情報テーブル
    // public DbSet<Author> Authors { get; set; }
    // // 例外情報テーブル
    // public DbSet<ErrorLog> ErrorLogs { get; set; }
    // // メタ情報テーブル
    // public DbSet<Meta> Metas { get; set; }
    // // 画像情報テーブル
    // public DbSet<Photo> Photos { get; set; }
}
