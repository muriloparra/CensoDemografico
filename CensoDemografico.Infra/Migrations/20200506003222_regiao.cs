using Microsoft.EntityFrameworkCore.Migrations;

namespace CensoDemografico.Infra.Migrations
{
    public partial class regiao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Regiao",
                table: "Pessoas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Regiao",
                table: "Pessoas");
        }
    }
}
