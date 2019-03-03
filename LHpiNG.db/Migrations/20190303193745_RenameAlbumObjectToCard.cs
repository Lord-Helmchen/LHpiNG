using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class RenameAlbumObjectToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectProductMaps",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "AlbumObjects",
                schema: "LHpi");

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "LHpi",
                columns: table => new
                {
                    OracleName = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Set = table.Column<string>(maxLength: 3, nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: false),
                    ObjectType = table.Column<int>(nullable: false),
                    Uid = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    Number = table.Column<string>(nullable: true),
                    CollNr = table.Column<int>(nullable: true),
                    Foilage = table.Column<int>(nullable: false),
                    RarityString = table.Column<string>(nullable: true),
                    Rarity = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => new { x.OracleName, x.Version, x.Set, x.ObjectType, x.Language });
                    table.UniqueConstraint("AK_Cards_Uid", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Cards_Languages_Language",
                        column: x => x.Language,
                        principalSchema: "LHpi",
                        principalTable: "Languages",
                        principalColumn: "TLA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Sets_Set",
                        column: x => x.Set,
                        principalSchema: "LHpi",
                        principalTable: "Sets",
                        principalColumn: "TLA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardProductMaps",
                schema: "LHpi",
                columns: table => new
                {
                    CardUid = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    ProductUid = table.Column<byte[]>(type: "binary(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardProductMaps", x => new { x.CardUid, x.ProductUid });
                    table.ForeignKey(
                        name: "FK_CardProductMaps_Cards_CardUid",
                        column: x => x.CardUid,
                        principalSchema: "LHpi",
                        principalTable: "Cards",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardProductMaps_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "LHpi",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardProductMaps_CardUid",
                schema: "LHpi",
                table: "CardProductMaps",
                column: "CardUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardProductMaps_ProductUid",
                schema: "LHpi",
                table: "CardProductMaps",
                column: "ProductUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Language",
                schema: "LHpi",
                table: "Cards",
                column: "Language");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Set",
                schema: "LHpi",
                table: "Cards",
                column: "Set");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardProductMaps",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "LHpi");

            migrationBuilder.CreateTable(
                name: "AlbumObjects",
                schema: "LHpi",
                columns: table => new
                {
                    OracleName = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Set = table.Column<string>(maxLength: 3, nullable: false),
                    ObjectType = table.Column<int>(nullable: false),
                    Language = table.Column<string>(maxLength: 3, nullable: false),
                    CollNr = table.Column<int>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Foilage = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Rarity = table.Column<int>(nullable: false),
                    RarityString = table.Column<string>(nullable: true),
                    Uid = table.Column<byte[]>(type: "binary(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumObjects", x => new { x.OracleName, x.Version, x.Set, x.ObjectType, x.Language });
                    table.UniqueConstraint("AK_AlbumObjects_Uid", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_AlbumObjects_Languages_Language",
                        column: x => x.Language,
                        principalSchema: "LHpi",
                        principalTable: "Languages",
                        principalColumn: "TLA",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlbumObjects_Sets_Set",
                        column: x => x.Set,
                        principalSchema: "LHpi",
                        principalTable: "Sets",
                        principalColumn: "TLA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectProductMaps",
                schema: "LHpi",
                columns: table => new
                {
                    AlbumObjectUid = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    ProductUid = table.Column<byte[]>(type: "binary(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectProductMaps", x => new { x.AlbumObjectUid, x.ProductUid });
                    table.ForeignKey(
                        name: "FK_ObjectProductMaps_AlbumObjects_AlbumObjectUid",
                        column: x => x.AlbumObjectUid,
                        principalSchema: "LHpi",
                        principalTable: "AlbumObjects",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjectProductMaps_Products_ProductUid",
                        column: x => x.ProductUid,
                        principalSchema: "LHpi",
                        principalTable: "Products",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumObjects_Language",
                schema: "LHpi",
                table: "AlbumObjects",
                column: "Language");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumObjects_Set",
                schema: "LHpi",
                table: "AlbumObjects",
                column: "Set");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProductMaps_AlbumObjectUid",
                schema: "LHpi",
                table: "ObjectProductMaps",
                column: "AlbumObjectUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObjectProductMaps_ProductUid",
                schema: "LHpi",
                table: "ObjectProductMaps",
                column: "ProductUid",
                unique: true);
        }
    }
}
