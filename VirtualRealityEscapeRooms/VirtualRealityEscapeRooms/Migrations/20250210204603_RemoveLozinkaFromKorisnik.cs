using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualRealityEscapeRooms.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLozinkaFromKorisnik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lozinka",
                table: "Korisnik");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lozinka",
                table: "Korisnik",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
