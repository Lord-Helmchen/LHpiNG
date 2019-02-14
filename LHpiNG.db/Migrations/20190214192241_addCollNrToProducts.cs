using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class addCollNrToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                schema: "LHpi",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollNr",
                schema: "LHpi",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollNr",
                schema: "LHpi",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                schema: "LHpi",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
