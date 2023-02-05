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
        }
    }
}
