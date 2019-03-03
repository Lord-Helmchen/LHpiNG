using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class RecreateFromScratch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "LHpi");

            migrationBuilder.CreateTable(
                name: "Expansions",
                schema: "LHpi",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    IdExpansion = table.Column<int>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<string>(nullable: true),
                    IsReleased = table.Column<bool>(nullable: true),
                    Uid = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProductCount = table.Column<int>(nullable: true),
                    ReleaseDateTime = table.Column<DateTime>(nullable: true),
                    UrlSuffix = table.Column<string>(nullable: true),
                    ProductsUrlSuffix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expansions", x => x.Name);
                    table.UniqueConstraint("AK_Expansions_Uid", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "LHpi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    TLA = table.Column<string>(maxLength: 3, nullable: false),
                    M15Abbr = table.Column<string>(maxLength: 2, nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.UniqueConstraint("AK_Languages_TLA", x => x.TLA);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                schema: "LHpi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    TLA = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    CardCount = table.Column<int>(nullable: true),
                    TokenCount = table.Column<int>(nullable: true),
                    NontraditionalCount = table.Column<int>(nullable: true),
                    InsertCount = table.Column<int>(nullable: true),
                    ReplicaCount = table.Column<int>(nullable: true),
                    Foilage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.UniqueConstraint("AK_Sets_TLA", x => x.TLA);
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "LHpi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExpansionListFetchDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "LHpi",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    ExpansionName = table.Column<string>(nullable: false),
                    IdProduct = table.Column<int>(nullable: true),
                    IdMetaproduct = table.Column<int>(nullable: true),
                    CountReprints = table.Column<int>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Rarity = table.Column<int>(nullable: false),
                    Uid = table.Column<byte[]>(type: "binary(32)", nullable: false),
                    CollNr = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => new { x.Name, x.ExpansionName });
                    table.UniqueConstraint("AK_Products_Uid", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Products_Expansions_ExpansionName",
                        column: x => x.ExpansionName,
                        principalSchema: "LHpi",
                        principalTable: "Expansions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumObjects",
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
                name: "SetExpansionMaps",
                schema: "LHpi",
                columns: table => new
                {
                    SetTLA = table.Column<string>(maxLength: 3, nullable: false),
                    ExpansionUid = table.Column<byte[]>(type: "binary(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetExpansionMaps", x => new { x.SetTLA, x.ExpansionUid });
                    table.ForeignKey(
                        name: "FK_SetExpansionMaps_Expansions_ExpansionUid",
                        column: x => x.ExpansionUid,
                        principalSchema: "LHpi",
                        principalTable: "Expansions",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetExpansionMaps_Sets_SetTLA",
                        column: x => x.SetTLA,
                        principalSchema: "LHpi",
                        principalTable: "Sets",
                        principalColumn: "TLA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceGuides",
                schema: "LHpi",
                columns: table => new
                {
                    FetchDate = table.Column<DateTime>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    ExpansionName = table.Column<string>(nullable: false),
                    Sell = table.Column<decimal>(nullable: true),
                    Low = table.Column<decimal>(nullable: true),
                    LowexPlus = table.Column<decimal>(nullable: true),
                    LowFoil = table.Column<decimal>(nullable: true),
                    Avg = table.Column<decimal>(nullable: true),
                    Trend = table.Column<decimal>(nullable: true),
                    IdProduct = table.Column<int>(nullable: true),
                    GermanProLow = table.Column<decimal>(nullable: true),
                    Suggested = table.Column<decimal>(nullable: true),
                    FoilSell = table.Column<decimal>(nullable: true),
                    FoilTrend = table.Column<decimal>(nullable: true),
                    Uid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceGuides", x => new { x.ProductName, x.ExpansionName, x.FetchDate });
                    table.UniqueConstraint("AK_PriceGuides_Uid", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_PriceGuides_Products_ProductName_ExpansionName",
                        columns: x => new { x.ProductName, x.ExpansionName },
                        principalSchema: "LHpi",
                        principalTable: "Products",
                        principalColumns: new[] { "Name", "ExpansionName" },
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

            migrationBuilder.InsertData(
                schema: "LHpi",
                table: "State",
                columns: new[] { "Id", "ExpansionListFetchDate" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

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

            migrationBuilder.CreateIndex(
                name: "IX_Products_ExpansionName",
                schema: "LHpi",
                table: "Products",
                column: "ExpansionName");

            migrationBuilder.CreateIndex(
                name: "IX_SetExpansionMaps_ExpansionUid",
                schema: "LHpi",
                table: "SetExpansionMaps",
                column: "ExpansionUid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetExpansionMaps_SetTLA",
                schema: "LHpi",
                table: "SetExpansionMaps",
                column: "SetTLA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sets_Name",
                schema: "LHpi",
                table: "Sets",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObjectProductMaps",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "PriceGuides",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "SetExpansionMaps",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "State",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "AlbumObjects",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Sets",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Expansions",
                schema: "LHpi");
        }
    }
}
