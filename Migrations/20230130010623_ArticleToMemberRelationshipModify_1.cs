using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class ArticleToMemberRelationshipModify1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_article_member_createby",
                table: "article");

            migrationBuilder.DropIndex(
                name: "IX_article_createby",
                table: "article");

            migrationBuilder.DropColumn(
                name: "createby",
                table: "article");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createby",
                table: "article",
                type: "varchar(50)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_article_createby",
                table: "article",
                column: "createby");

            migrationBuilder.AddForeignKey(
                name: "FK_article_member_createby",
                table: "article",
                column: "createby",
                principalTable: "member",
                principalColumn: "member_id");
        }
    }
}
