using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace boarddotnet.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pwd = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nickname = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_member", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "varchar(2000)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    viewcount = table.Column<int>(name: "view_count", type: "int", nullable: false),
                    hashtag = table.Column<string>(name: "hash_tag", type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createby = table.Column<long>(name: "create_by", type: "bigint", nullable: false),
                    createat = table.Column<DateTime>(name: "create_at", type: "datetime(6)", nullable: false),
                    updateby = table.Column<long>(name: "update_by", type: "bigint", nullable: false),
                    updateat = table.Column<DateTime>(name: "update_at", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => x.id);
                    table.ForeignKey(
                        name: "FK_article_member_create_by",
                        column: x => x.createby,
                        principalTable: "member",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "attachFile",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    articleid = table.Column<long>(name: "article_id", type: "bigint", nullable: false),
                    fileName = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    blobName = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createby = table.Column<long>(name: "create_by", type: "bigint", nullable: false),
                    createat = table.Column<DateTime>(name: "create_at", type: "datetime(6)", nullable: false),
                    updateby = table.Column<long>(name: "update_by", type: "bigint", nullable: false),
                    updateat = table.Column<DateTime>(name: "update_at", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachFile", x => x.id);
                    table.ForeignKey(
                        name: "FK_attachFile_article_article_id",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    articleid = table.Column<long>(name: "article_id", type: "bigint", nullable: false),
                    comment = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createby = table.Column<long>(name: "create_by", type: "bigint", nullable: false),
                    createat = table.Column<DateTime>(name: "create_at", type: "datetime(6)", nullable: false),
                    updateby = table.Column<long>(name: "update_by", type: "bigint", nullable: false),
                    updateat = table.Column<DateTime>(name: "update_at", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_article_article_id",
                        column: x => x.articleid,
                        principalTable: "article",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_member_create_by",
                        column: x => x.createby,
                        principalTable: "member",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_article_content",
                table: "article",
                column: "content");

            migrationBuilder.CreateIndex(
                name: "IX_article_create_at",
                table: "article",
                column: "create_at");

            migrationBuilder.CreateIndex(
                name: "IX_article_create_by",
                table: "article",
                column: "create_by");

            migrationBuilder.CreateIndex(
                name: "IX_article_title",
                table: "article",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "IX_attachFile_article_id",
                table: "attachFile",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_article_id",
                table: "comment",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_comment",
                table: "comment",
                column: "comment");

            migrationBuilder.CreateIndex(
                name: "IX_comment_create_at",
                table: "comment",
                column: "create_at");

            migrationBuilder.CreateIndex(
                name: "IX_comment_create_by",
                table: "comment",
                column: "create_by");

            migrationBuilder.CreateIndex(
                name: "IX_member_email",
                table: "member",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachFile");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "member");
        }
    }
}
