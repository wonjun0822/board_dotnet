using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class AlterAttachFileAddIndexContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "article",
                type: "varchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_article_content",
                table: "article",
                column: "content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_article_content",
                table: "article");

            migrationBuilder.AlterColumn<string>(
                name: "content",
                table: "article",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
