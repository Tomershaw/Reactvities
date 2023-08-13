using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhotos123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_photos_AspNetUsers_AppUserId",
                table: "photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_photos",
                table: "photos");

            migrationBuilder.RenameTable(
                name: "photos",
                newName: "Photos123");

            migrationBuilder.RenameIndex(
                name: "IX_photos_AppUserId",
                table: "Photos123",
                newName: "IX_Photos123_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos123",
                table: "Photos123",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos123_AspNetUsers_AppUserId",
                table: "Photos123",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos123_AspNetUsers_AppUserId",
                table: "Photos123");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos123",
                table: "Photos123");

            migrationBuilder.RenameTable(
                name: "Photos123",
                newName: "photos");

            migrationBuilder.RenameIndex(
                name: "IX_Photos123_AppUserId",
                table: "photos",
                newName: "IX_photos_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_photos",
                table: "photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_photos_AspNetUsers_AppUserId",
                table: "photos",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
