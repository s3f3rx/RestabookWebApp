using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestabookWebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitAboutPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundPhoto",
                table: "Abouts");

            migrationBuilder.RenameColumn(
                name: "MainPhoto",
                table: "Abouts",
                newName: "PhotoPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Abouts",
                newName: "MainPhoto");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundPhoto",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
