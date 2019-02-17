using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LHpiNG.db.Migrations
{
    public partial class AddAlbumObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "LHpi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Abbr = table.Column<string>(nullable: true),
                    M15Abbr = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
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
                name: "AlbumObjects",
                schema: "LHpi",
                columns: table => new
                {
                    OracleName = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Set = table.Column<string>(nullable: false),
                    Language = table.Column<int>(nullable: false),
                    Uid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(nullable: true),
                    CollNr = table.Column<int>(nullable: true),
                    Foilage = table.Column<int>(nullable: false),
                    Rarity = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumObjects", x => new { x.OracleName, x.Version, x.Set, x.Language });
                    table.UniqueConstraint("AK_AlbumObjects_Uid", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_AlbumObjects_Languages_Language",
                        column: x => x.Language,
                        principalSchema: "LHpi",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlbumObjects_Sets_Set",
                        column: x => x.Set,
                        principalSchema: "LHpi",
                        principalTable: "Sets",
                        principalColumn: "TLA",
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
                name: "IX_Sets_Name",
                schema: "LHpi",
                table: "Sets",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumObjects",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "LHpi");

            migrationBuilder.DropTable(
                name: "Sets",
                schema: "LHpi");
        }
    }
}
