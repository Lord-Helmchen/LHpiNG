using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class AddMaps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObjectProductMap",
                schema: "LHpi",
                columns: table => new
                {
                    AlbumObjectUid = table.Column<int>(nullable: false),
                    ProductUid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectProductMap", x => new { x.AlbumObjectUid, x.ProductUid });
                    table.ForeignKey(
                        name: "FK_ObjectProductMap_AlbumObjects_AlbumObjectUid",
                        column: x => x.AlbumObjectUid,
                        principalSchema: "LHpi",
                        principalTable: "AlbumObjects",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectProductMap_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "LHpi",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetExpansionMap",
                schema: "LHpi",
                columns: table => new
                {
                    SetTLA = table.Column<string>(maxLength: 3, nullable: false),
                    ExpansionEnName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetExpansionMap", x => new { x.SetTLA, x.ExpansionEnName });
                    table.ForeignKey(
                        name: "FK_SetExpansionMap_Expansions_ExpansionEnName",
                        column: x => x.ExpansionEnName,
                        principalSchema: "LHpi",
                        principalTable: "Expansions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetExpansionMap_Sets_SetTLA",
                        column: x => x.SetTLA,
                        principalSchema: "LHpi",
                        principalTable: "Sets",
                        principalColumn: "TLA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProductMap_AlbumObjectUid",
                schema: "LHpi",
                table: "ObjectProductMap",
                column: "AlbumObjectUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProductMap_ProductUid",
                schema: "LHpi",
                table: "ObjectProductMap",
                column: "ProductUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetExpansionMap_ExpansionEnName",
                schema: "LHpi",
                table: "SetExpansionMap",
                column: "ExpansionEnName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetExpansionMap_SetTLA",
                schema: "LHpi",
                table: "SetExpansionMap",
                column: "SetTLA",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectProductMap",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "SetExpansionMap",
                schema: "LHpi");
        }
    }
}
