using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentarySystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "file",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_name",
                table: "file");
        }
    }
}
