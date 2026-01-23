using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreEntity.Migrations
{
    /// <inheritdoc />
    public partial class Mod_Review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Contents_ForBook",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Comments",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ForBook",
                table: "Comments",
                newName: "IX_Comments_ForBook");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Comments",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Contents_ForBook",
                table: "Comments",
                column: "ForBook",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Contents_ForBook",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Reviews");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Reviews",
                newName: "Body");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ForBook",
                table: "Reviews",
                newName: "IX_Reviews_ForBook");

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Contents_ForBook",
                table: "Reviews",
                column: "ForBook",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
