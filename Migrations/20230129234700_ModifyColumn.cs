using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class ModifyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "update_id",
                table: "comment",
                newName: "update_by");

            migrationBuilder.RenameColumn(
                name: "update_date",
                table: "comment",
                newName: "update_at");

            migrationBuilder.RenameColumn(
                name: "create_id",
                table: "comment",
                newName: "create_by");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "comment",
                newName: "create_at");

            migrationBuilder.RenameIndex(
                name: "IX_comment_create_id",
                table: "comment",
                newName: "IX_comment_create_by");

            migrationBuilder.RenameIndex(
                name: "IX_comment_create_date",
                table: "comment",
                newName: "IX_comment_create_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "update_by",
                table: "comment",
                newName: "update_id");

            migrationBuilder.RenameColumn(
                name: "update_at",
                table: "comment",
                newName: "update_date");

            migrationBuilder.RenameColumn(
                name: "create_by",
                table: "comment",
                newName: "create_id");

            migrationBuilder.RenameColumn(
                name: "create_at",
                table: "comment",
                newName: "create_date");

            migrationBuilder.RenameIndex(
                name: "IX_comment_create_by",
                table: "comment",
                newName: "IX_comment_create_id");

            migrationBuilder.RenameIndex(
                name: "IX_comment_create_at",
                table: "comment",
                newName: "IX_comment_create_date");
        }
    }
}
