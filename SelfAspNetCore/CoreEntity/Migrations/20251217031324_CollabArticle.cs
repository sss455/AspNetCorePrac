using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreEntity.Migrations
{
    /// <inheritdoc />
    public partial class CollabArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Articles",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Articles");
        }
    }
}
