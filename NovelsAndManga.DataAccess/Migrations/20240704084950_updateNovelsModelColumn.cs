using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelsManga.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateNovelsModelColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "seriesAuthor",
                table: "NovelsPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "seriesStatus",
                table: "NovelsPage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seriesAuthor",
                table: "NovelsPage");

            migrationBuilder.DropColumn(
                name: "seriesStatus",
                table: "NovelsPage");
        }
    }
}
