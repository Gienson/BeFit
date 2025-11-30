using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeFit.Data.Migrations
{
    public partial class FinalFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BestResult",
                table: "Stat",
                newName: "TotalReps");

            migrationBuilder.AddColumn<double>(
                name: "AvgWeight",
                table: "Stat",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MaxWeight",
                table: "Stat",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgWeight",
                table: "Stat");

            migrationBuilder.DropColumn(
                name: "MaxWeight",
                table: "Stat");

            migrationBuilder.RenameColumn(
                name: "TotalReps",
                table: "Stat",
                newName: "BestResult");
        }
    }
}
