using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtualRealityEscapeRooms.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRegistracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Korisnik",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_UserId",
                table: "Korisnik",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnik_AspNetUsers_UserId",
                table: "Korisnik",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnik_AspNetUsers_UserId",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "IX_Korisnik_UserId",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Korisnik");
        }
    }
}
