using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelsManga.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateNovelsChapterValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NovelsChapters",
                columns: table => new
                {
                    novelChapterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chapterNum = table.Column<int>(type: "int", nullable: false),
                    chapterContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NovelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovelsChapters", x => x.novelChapterId);
                    table.ForeignKey(
                        name: "FK_NovelsChapters_NovelsPage_NovelsId",
                        column: x => x.NovelsId,
                        principalTable: "NovelsPage",
                        principalColumn: "NovelsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NovelsChapters_NovelsId",
                table: "NovelsChapters",
                column: "NovelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NovelsChapters");
        }
    }
}
