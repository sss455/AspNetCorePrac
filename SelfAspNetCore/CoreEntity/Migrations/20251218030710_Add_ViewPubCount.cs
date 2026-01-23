using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreEntity.Migrations
{
    /// <inheritdoc />
    public partial class Add_ViewPubCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ViewPubCounts AS 
                                     SELECT b.Publisher, COUNT(*) as BookCount
                                     FROM Contents b 
                                     GROUP BY b.Publisher");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW ViewPubCounts");
        }
    }
}
