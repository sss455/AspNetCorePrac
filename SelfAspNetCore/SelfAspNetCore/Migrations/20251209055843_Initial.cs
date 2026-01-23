using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfAspNetCore.Migrations
{
    
    // p.248 マイグレーションファイルの構造
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        // Upメソッド：スキーマの状態を更新するためのメソッド
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                // テーブル名
                name: "Books",
                // 列を定義するための処理「Func<ColumnsBuilder, TColumns>」
                columns: table => new
                {
                 // 列名                   <データ型>   DB上の型名             NULL値を許可するか
                    Id        = table.Column<int>     (type: "int",           nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Isbn      = table.Column<string>  (type: "nvarchar(max)", nullable: false),
                    Title     = table.Column<string>  (type: "nvarchar(max)", nullable: false),
                    Price     = table.Column<int>     (type: "int",           nullable: false),
                    Publisher = table.Column<string>  (type: "nvarchar(max)", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2",     nullable: false),
                    Sample    = table.Column<bool>    (type: "bit",           nullable: false)
                },
                // 制約を生成するための処理
                constraints: table =>   
                {
                    // 主キー制約      主キー名    対象列
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        // スキーマの状態を1つ前に戻すためのメソッド（特定のタイミングの状態に戻せる）
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
