using CoreEntity.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreEntity.Models;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options) { }
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<Article> Articles { get; set; } = default!;
    public DbSet<CollabArticle> CollabArticles { get; set; } = default!;
    public DbSet<ViewPubCount> ViewPubCounts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ViewPubCount>()
          .ToView("ViewPubCounts");

        // builder.Entity<Review>(rev =>
        // {
        //     rev.ToTable("Comments")
        //        .HasKey(b => b.Code);
        //     rev.Property(e => e.Body)
        //        .HasColumnName("Message")
        //        .HasMaxLength(150);
        // });

        // new ReviewEntityTypeConfiguration().Configure(
        //     builder.Entity<Review>());

        // builder.Entity<User>(e =>
        // {
        //     e.Property(e => e.UserClass)
        //      .HasConversion(
        //        v => v.ToString(),
        //        v => (UserClass)Enum.Parse(typeof(UserClass), v)
        //      );
        // });

        // builder.Entity<User>(e => {
        //     e.Property(e => e.Email)
        //     .HasConversion(new ValueConverter<EmailAddress?, string>(
        //     v => v == null ? "": v.ToString(),
        //     v => new EmailAddress(v))
        //     );
        // });

        builder.Entity<Book>(e =>
        {
            e.HasData(
              new Book
              {
                  Id = 1,
                  Isbn = "978-4-7981-8094-6",
                  Title = "独習Java",
                  Price = 3960,
                  Publisher = "翔泳社",
                  Published = new DateTime(2024, 02, 15),
                  Sample = true
              },
              new Book
              {
                  Id = 2,
                  Isbn = "978-4-7981-7613-0",
                  Title = "Androidアプリ開発の教科書",
                  Price = 3135,
                  Publisher = "翔泳社",
                  Published = new DateTime(2023, 01, 24),
                  Sample = true
              },
              new Book
              {
                  Id = 3,
                  Isbn = "978-4-8156-1948-0",
                  Title = "これからはじめるReact実践入門",
                  Price = 4400,
                  Publisher = "SBクリエイティブ",
                  Published = new DateTime(2023, 09, 28),
                  Sample = true
              },
              new Book
              {
                  Id = 4,
                  Isbn = "978-4-296-07070-1",
                  Title = "作って学べるHTML＋JavaScriptの基本",
                  Price = 2420,
                  Publisher = "日経BP",
                  Published = new DateTime(2023, 07, 06),
                  Sample = false
              },
              new Book
              {
                  Id = 5,
                  Isbn = "978-4-297-13685-7",
                  Title = "Nuxt 3 フロントエンド開発の教科書",
                  Price = 3520,
                  Publisher = "技術評論社",
                  Published = new DateTime(2023, 09, 22),
                  Sample = false
              },
              new Book
              {
                  Id = 6,
                  Isbn = "978-4-297-13288-0",
                  Title = "JavaScript本格入門",
                  Price = 3520,
                  Publisher = "技術評論社",
                  Published = new DateTime(2023, 02, 13),
                  Sample = true
              },
              new Book
              {
                  Id = 7,
                  Isbn = "978-4-627-85711-7",
                  Title = "Pythonでできる! 株価データ分析",
                  Price = 2970,
                  Publisher = "森北出版",
                  Published = new DateTime(2023, 01, 21),
                  Sample = false
              },
              new Book
              {
                  Id = 8,
                  Isbn = "978-4-7981-7556-0",
                  Title = "独習C#",
                  Price = 4180,
                  Publisher = "翔泳社",
                  Published = new DateTime(2022, 07, 21),
                  Sample = true
              },
              new Book
              {
                  Id = 9,
                  Isbn = "978-4-8156-1336-5",
                  Title = "これからはじめるVue.js 3実践入門",
                  Price = 3740,
                  Publisher = "SBクリエイティブ",
                  Published = new DateTime(2022, 03, 19),
                  Sample = true
              },
              new Book
              {
                  Id = 10,
                  Isbn = "978-4-296-08014-4",
                  Title = "基礎からしっかり学ぶC#の教科書",
                  Price = 3190,
                  Publisher = "日経BP",
                  Published = new DateTime(2022, 03, 03),
                  Sample = false
              }
            );
        });

        builder.Entity<User>()
            .HasData(
                new User { Id = 1, LastName = "山田", FirstName = "祥寛", Email = new EmailAddress("yyamada@example.com"), Birth = new DateTime(1980, 01, 01) },
                new User { Id = 2, LastName = "山内", FirstName = "直", Email =  new EmailAddress("nyamauchi@example.com"), Birth = new DateTime(1970, 04, 10) },
                new User { Id = 3, LastName = "鈴木", FirstName = "花子", Email =  new EmailAddress("hsuzuki@example.com"), Birth = new DateTime(1990, 11, 24) },
                new User { Id = 4, LastName = "佐藤", FirstName = "太郎", Email =  new EmailAddress("tsato@example.com"), Birth = new DateTime(1992, 08, 21) },
                new User { Id = 5, LastName = "片渕", FirstName = "彼富", Email =  new EmailAddress("kkatafuchi@example.com"), Birth = new DateTime(1985, 10, 20) },
                new User { Id = 6, LastName = "齊藤", FirstName = "新三", Email =  new EmailAddress("ssaito@example.com"), Birth = new DateTime(1974, 02, 15) },
                new User { Id = 7, LastName = "鈴木", FirstName = "花子", Email =  new EmailAddress("shanako@example.com"), Birth = new DateTime(2000, 12, 01) },
                new User { Id = 8, LastName = "高江", FirstName = "賢", Email =  new EmailAddress("ktakae@example.com"), Birth = new DateTime(1978, 06, 30) }
            );

        builder.Entity<Author>()
            .HasData(
                new Author { Id = 1, PenName = "WINGS", UserId = 1 },
                new Author { Id = 2, PenName = "Sarva", UserId = 6 },
                new Author { Id = 3, PenName = "たまデジ。", UserId = 2 },
                new Author { Id = 4, PenName = "Canon", UserId = 5 },
                new Author { Id = 5, PenName = "Papier", UserId = 8 },
                new Author { Id = 6, PenName = "はな", UserId = 3 }
            );

        builder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithMany(b => b.Authors)
            .UsingEntity<Dictionary<string, object>>(
                "AuthorBook",
                r => r.HasOne<Book>().WithMany().HasForeignKey("BooksId"),
                l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorsId"),
                je =>
                {
                    je.HasKey("BooksId", "AuthorsId");
                    je.HasData(
                        new { BooksId = 1, AuthorsId = 1 },
                        new { BooksId = 1, AuthorsId = 6 },
                        new { BooksId = 2, AuthorsId = 2 },
                        new { BooksId = 3, AuthorsId = 1 },
                        new { BooksId = 4, AuthorsId = 3 },
                        new { BooksId = 5, AuthorsId = 2 },
                        new { BooksId = 6, AuthorsId = 1 },
                        new { BooksId = 7, AuthorsId = 4 },
                        new { BooksId = 8, AuthorsId = 1 },
                        new { BooksId = 9, AuthorsId = 1 },
                        new { BooksId = 10, AuthorsId = 5 }
                    );
                }
            );

        builder.Entity<Review>()
            .HasData(
                new Review
                {
                    Code = 1,
                    Name = "山田太郎",
                    Body = "がっつり学習したい人には、うってつけの本です。前半は、キッチリ基礎固めを行い、後半は応用的な内容に。図や用例、構文も多く、辞書代わりにも使えます。",
                    LastUpdated = new DateTime(2024, 02, 20),
                    ForBook = 1
                },
                new Review
                {
                    Code = 2,
                    Name = "佐藤花子",
                    Body = "環境構築から書かれており初心者から使えますが、分量から見てもわかるように、サラッとは終わらず、説明が深いです。特に「エキスパートに訊く」という読み物が面白い。",
                    LastUpdated = new DateTime(2024, 03, 01),
                    ForBook = 1
                },
                new Review
                {
                    Code = 3,
                    Name = "鈴木次郎",
                    Body = "コツコツ、独りでじっくり勉強できます。オブジェクト指向プログラミングに紙数が割いてあって、以前、挫折したけど、今回は理解できました。1回目は読み飛ばしたところもあるので、また読み返したいです。",
                    LastUpdated = new DateTime(2024, 03, 10),
                    ForBook = 1
                },
                new Review
                {
                    Code = 4,
                    Name = "井上裕子",
                    Body = "Androidはすぐにバージョンが上がるので、本を買うのもためらわれるが、この本は、基礎部分がしっかり説明されているので、はじめの一冊にお勧めできます。",
                    LastUpdated = new DateTime(2023, 11, 01),
                    ForBook = 2
                },
                new Review
                {
                    Code = 5,
                    Name = "田中健太",
                    Body = "同様の構成で、Kotlin対応とJava対応があるので、両方手元にあると比較しながら勉強できます。",
                    LastUpdated = new DateTime(2023, 11, 30),
                    ForBook = 2
                },
                new Review
                {
                    Code = 6,
                    Name = "藤井由美",
                    Body = "テスト、TypeScriptの活用、Next.jsの活用などなど。実践的な内容盛りだくさんで、かなりのボリュームです。サンプルも多く、ダウンロードして実際に動かしながら確認できて、理解が深まります。",
                    LastUpdated = new DateTime(2023, 12, 10),
                    ForBook = 2
                }
            );
    }

    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
          .Properties<EmailAddress>()
          .HaveConversion<EmailAddressConverter>();
    }
}