using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentarySystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedWayOfSavingFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_path",
                table: "file");

            migrationBuilder.AddColumn<byte[]>(
                name: "content",
                table: "file",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "file");

            migrationBuilder.AddColumn<string>(
                name: "file_path",
                table: "file",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
