using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos123_AspNetUsers_AppUserId",
                table: "Photos123");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos123",
                table: "Photos123");

            migrationBuilder.RenameTable(
                name: "Photos123",
                newName: "Photos");

            migrationBuilder.RenameIndex(
                name: "IX_Photos123_AppUserId",
                table: "Photos",
                newName: "IX_Photos_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Photos",
                table: "Photos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_AspNetUsers_AppUserId",
                table: "Photos",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_AspNetUsers_AppUserId",
                table: "Photos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Photos",
                table: "Photos");

            migrationBuilder.RenameTable(
                name: "Photos",
                newName: "Photos123");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_AppUserId",
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
    }
}
