using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class AlterAttachFileAddblobNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "blobName",
                table: "attachFile",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blobName",
                table: "attachFile");
        }
    }
}
