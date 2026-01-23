using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chapter07.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sample = table.Column<bool>(type: "bit", nullable: false),
                    RowVewsion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stacktrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accessed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NeedNews = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    AuthorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.BooksId, x.AuthorsId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Category", "CreatedAt", "LastUpdatedAt", "Title", "Url" },
                values: new object[,]
                {
                    { 1, "JavaScript", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "ますます便利になるTypeScript！", "https://codezine.jp/article/corner/992" },
                    { 2, "JavaScript", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Remixを通じてWebを学ぶ", "https://codezine.jp/article/corner/942" },
                    { 3, "JavaScript", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Componentsを基礎から学ぶ", "https://codezine.jp/article/corner/927" },
                    { 4, "Rails", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Railsの新機能を知ろう！", "https://codezine.jp/article/corner/991" },
                    { 5, "Rails", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Railsによるクライアントサイド開発入門", "https://codezine.jp/article/corner/919" },
                    { 6, "Rust", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "現場で役立つRust入門", "https://atmarkit.itmedia.co.jp/ait/series/36943/" },
                    { 7, "Rust", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "基本からしっかり学ぶRust入門", "https://atmarkit.itmedia.co.jp/ait/series/24844/" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Isbn", "Price", "Published", "Publisher", "Sample", "Title" },
                values: new object[,]
                {
                    { 1, "978-4-7981-8094-6", 3960, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "翔泳社", true, "独習Java" },
                    { 2, "978-4-7981-7613-0", 3135, new DateTime(2023, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "翔泳社", true, "Androidアプリ開発の教科書" },
                    { 3, "978-4-8156-1948-0", 4400, new DateTime(2023, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "SBクリエイティブ", true, "これからはじめるReact実践入門" },
                    { 4, "978-4-296-07070-1", 2420, new DateTime(2023, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "日経BP", false, "作って学べるHTML＋JavaScriptの基本" },
                    { 5, "978-4-297-13685-7", 3520, new DateTime(2023, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "技術評論社", false, "Nuxt 3 フロントエンド開発の教科書" },
                    { 6, "978-4-297-13288-0", 3520, new DateTime(2023, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "技術評論社", true, "JavaScript本格入門" },
                    { 7, "978-4-627-85711-7", 2970, new DateTime(2023, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "森北出版", false, "Pythonでできる! 株価データ分析" },
                    { 8, "978-4-7981-7556-0", 4180, new DateTime(2022, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "翔泳社", true, "独習C#" },
                    { 9, "978-4-8156-1336-5", 3740, new DateTime(2022, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "SBクリエイティブ", true, "これからはじめるVue.js 3実践入門" },
                    { 10, "978-4-296-08014-4", 3190, new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "日経BP", false, "基礎からしっかり学ぶC#の教科書" }
                });

            migrationBuilder.InsertData(
                table: "Metas",
                columns: new[] { "Id", "Action", "Content", "Controller", "Name" },
                values: new object[,]
                {
                    { 1, "Privacy", "メタ情報", "Home", "keywords" },
                    { 2, "Privacy", "ページごとに異なる<meta>要素を生成", "Home", "description" },
                    { 3, "Index", "メタ情報取得の別解", "Tag", "description" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birth", "Email", "Name", "NeedNews" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "yyamada@example.com", "山田祥寛", false },
                    { 2, new DateTime(1970, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "nyamauchi@example.com", "山内直", false },
                    { 3, new DateTime(1990, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "hsuzuki@example.com", "鈴木花子", false },
                    { 4, new DateTime(1992, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "tsato@example.com", "佐藤太郎", false },
                    { 5, new DateTime(1985, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "kkatafuchi@example.com", "片渕彼富", false },
                    { 6, new DateTime(1974, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "ssaito@example.com", "齊藤新三", false },
                    { 7, new DateTime(2000, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shanako@example.com", "鈴木花子", false },
                    { 8, new DateTime(1978, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "ktakae@example.com", "高江賢", false }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "PenName", "UserId" },
                values: new object[,]
                {
                    { 1, "WINGS", 1 },
                    { 2, "Sarva", 6 },
                    { 3, "たまデジ。", 2 },
                    { 4, "Canon", 5 },
                    { 5, "Papier", 8 },
                    { 6, "はな", 3 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Body", "BookId", "LastUpdated", "Name" },
                values: new object[,]
                {
                    { 1, "がっつり学習したい人には、うってつけの本です。前半は、キッチリ基礎固めを行い、後半は応用的な内容に。図や用例、構文も多く、辞書代わりにも使えます。", 1, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "山田太郎" },
                    { 2, "環境構築から書かれており初心者から使えますが、分量から見てもわかるように、サラッとは終わらず、説明が深いです。特に「エキスパートに訊く」という読み物が面白い。", 1, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "佐藤花子" },
                    { 3, "コツコツ、独りでじっくり勉強できます。オブジェクト指向プログラミングに紙数が割いてあって、以前、挫折したけど、今回は理解できました。1回目は読み飛ばしたところもあるので、また読み返したいです。", 1, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "鈴木次郎" },
                    { 4, "Androidはすぐにバージョンが上がるので、本を買うのもためらわれるが、この本は、基礎部分がしっかり説明されているので、はじめの一冊にお勧めできます。", 2, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "井上裕子" },
                    { 5, "同様の構成で、Kotlin対応とJava対応があるので、両方手元にあると比較しながら勉強できます。", 2, new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "田中健太" },
                    { 6, "テスト、TypeScriptの活用、Next.jsの活用などなど。実践的な内容盛りだくさんで、かなりのボリュームです。サンプルも多く、ダウンロードして実際に動かしながら確認できて、理解が深まります。", 2, new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "藤井由美" }
                });

            migrationBuilder.InsertData(
                table: "AuthorBook",
                columns: new[] { "AuthorsId", "BooksId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 6, 1 },
                    { 2, 2 },
                    { 1, 3 },
                    { 3, 4 },
                    { 2, 5 },
                    { 1, 6 },
                    { 4, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 5, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_AuthorsId",
                table: "AuthorBook",
                column: "AuthorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "Metas");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
