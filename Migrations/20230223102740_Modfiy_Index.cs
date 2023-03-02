using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class ModfiyIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_comment_comment",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_create_at",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_article_create_at",
                table: "article");

            migrationBuilder.CreateIndex(
                name: "IX_member_nickname",
                table: "member",
                column: "nickname");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_member_nickname",
                table: "member");

            migrationBuilder.CreateIndex(
                name: "IX_comment_comment",
                table: "comment",
                column: "comment");

            migrationBuilder.CreateIndex(
                name: "IX_comment_create_at",
                table: "comment",
                column: "create_at");

            migrationBuilder.CreateIndex(
                name: "IX_article_create_at",
                table: "article",
                column: "create_at");
        }
    }
}
