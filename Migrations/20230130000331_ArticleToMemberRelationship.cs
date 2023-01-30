using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class ArticleToMemberRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_article_member_create_by",
                table: "article",
                column: "create_by",
                principalTable: "member",
                principalColumn: "member_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_article_member_createby",
                table: "article",
                column: "createby",
                principalTable: "member",
                principalColumn: "member_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_article_member_create_by",
                table: "article");

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
    }
}
