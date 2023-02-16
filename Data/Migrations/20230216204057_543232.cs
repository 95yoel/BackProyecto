using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsturTravel.Data.Migrations
{
    public partial class _543232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RutaImagen",
                table: "Destinos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RutaImagen",
                table: "Destinos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
