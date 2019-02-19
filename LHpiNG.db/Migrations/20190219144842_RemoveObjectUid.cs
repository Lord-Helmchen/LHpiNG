using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class RemoveObjectUid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AlbumObjects_Uid",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "LHpi",
                table: "AlbumObjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Uid",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AlbumObjects_Uid",
                schema: "LHpi",
                table: "AlbumObjects",
                column: "Uid");
        }
    }
}
