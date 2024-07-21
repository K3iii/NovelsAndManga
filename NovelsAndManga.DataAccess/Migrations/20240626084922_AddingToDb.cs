using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelsManga.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "NovelsPage",
                columns: table => new
                {
                    NovelsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NovelsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NovelsType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NovelsSummary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelsPage", x => x.NovelsId);
                });

            migrationBuilder.CreateTable(
                name: "novelsGenres",
                columns: table => new
                {
                    NovelsId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_novelsGenres", x => new { x.NovelsId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_novelsGenres_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_novelsGenres_NovelsPage_NovelsId",
                        column: x => x.NovelsId,
                        principalTable: "NovelsPage",
                        principalColumn: "NovelsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_novelsGenres_GenreId",
                table: "novelsGenres",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "novelsGenres");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "NovelsPage");
        }
    }
}
