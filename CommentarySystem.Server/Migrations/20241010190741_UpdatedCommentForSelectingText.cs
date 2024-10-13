using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentarySystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommentForSelectingText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_index",
                table: "comment");

            migrationBuilder.DropColumn(
                name: "start_index",
                table: "comment");

            migrationBuilder.CreateTable(
                name: "selected_range",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    start_index = table.Column<int>(type: "INTEGER", nullable: false),
                    end_index = table.Column<int>(type: "INTEGER", nullable: false),
                    comment_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_selected_range", x => x.id);
                    table.ForeignKey(
                        name: "fk_selected_range_comment_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comment",
                        principalColumn: "comment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_selected_range_comment_id",
                table: "selected_range",
                column: "comment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "selected_range");

            migrationBuilder.AddColumn<int>(
                name: "end_index",
                table: "comment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "start_index",
                table: "comment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
