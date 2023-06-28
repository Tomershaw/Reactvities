using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Veume",
                table: "Activities",
                newName: "Venue");

            migrationBuilder.RenameColumn(
                name: "Catgory",
                table: "Activities",
                newName: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Venue",
                table: "Activities",
                newName: "Veume");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Activities",
                newName: "Catgory");
        }
    }
}
