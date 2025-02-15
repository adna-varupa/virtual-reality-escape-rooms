using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualRealityEscapeRooms.Migrations
{
    /// <inheritdoc />
    public partial class SlikaPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slika");

            migrationBuilder.AddColumn<string>(
                name: "SlikaPath",
                table: "Igra",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlikaPath",
                table: "Igra");

            migrationBuilder.CreateTable(
                name: "Slika",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slika", x => x.ID);
                });
        }
    }
}
