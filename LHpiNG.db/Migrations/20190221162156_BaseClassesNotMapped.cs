using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class BaseClassesNotMapped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceGuides",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.DropIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName_FetchDate",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "LHpi",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.AddColumn<int>(
                name: "Uid",
                schema: "LHpi",
                table: "Products",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FetchDate",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpansionName",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Uid",
                schema: "LHpi",
                table: "AlbumObjects",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Uid",
                schema: "LHpi",
                table: "Products",
                column: "Uid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PriceGuides_Uid",
                schema: "LHpi",
                table: "PriceGuides",
                column: "Uid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceGuides",
                schema: "LHpi",
                table: "PriceGuides",
                columns: new[] { "ProductName", "ExpansionName", "FetchDate" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AlbumObjects_Uid",
                schema: "LHpi",
                table: "AlbumObjects",
                column: "Uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Uid",
                schema: "LHpi",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PriceGuides_Uid",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceGuides",
                schema: "LHpi",
                table: "PriceGuides");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AlbumObjects_Uid",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "LHpi",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Uid",
                schema: "LHpi",
                table: "AlbumObjects");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "LHpi",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FetchDate",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "ExpansionName",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "LHpi",
                table: "PriceGuides",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceGuides",
                schema: "LHpi",
                table: "PriceGuides",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_PriceGuides_ProductName_ExpansionName_FetchDate",
                schema: "LHpi",
                table: "PriceGuides",
                columns: new[] { "ProductName", "ExpansionName", "FetchDate" },
                unique: true,
                filter: "FetchDate not null and ProductName not null and ExpansionName not null");
        }
    }
}
