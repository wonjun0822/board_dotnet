using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_article_member_create_by",
                table: "article");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_member_create_by",
                table: "comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_member",
                table: "member");

            migrationBuilder.DropColumn(
                name: "member_id",
                table: "member");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "member",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "member",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "update_by",
                table: "comment",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "create_by",
                table: "comment",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "update_by",
                table: "attachFile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "create_by",
                table: "attachFile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_member",
                table: "member",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_member_email",
                table: "member",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_article_member_create_by",
                table: "article",
                column: "create_by",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_member_create_by",
                table: "comment",
                column: "create_by",
                principalTable: "member",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_article_member_create_by",
                table: "article");

            migrationBuilder.DropForeignKey(
                name: "FK_comment_member_create_by",
                table: "comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_member",
                table: "member");

            migrationBuilder.DropIndex(
                name: "IX_member_email",
                table: "member");

            migrationBuilder.DropColumn(
                name: "id",
                table: "member");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "member",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "member_id",
                table: "member",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "update_by",
                table: "comment",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "create_by",
                table: "comment",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "update_by",
                table: "attachFile",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "create_by",
                table: "attachFile",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_member",
                table: "member",
                column: "member_id");

            migrationBuilder.AddForeignKey(
                name: "FK_article_member_create_by",
                table: "article",
                column: "create_by",
                principalTable: "member",
                principalColumn: "member_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_member_create_by",
                table: "comment",
                column: "create_by",
                principalTable: "member",
                principalColumn: "member_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
