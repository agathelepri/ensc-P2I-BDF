using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Familles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    CouleurHexa = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Annee = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eleves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nom = table.Column<string>(type: "TEXT", nullable: true),
                    Prenom = table.Column<string>(type: "TEXT", nullable: true),
                    Login = table.Column<string>(type: "TEXT", nullable: true),
                    MDP = table.Column<string>(type: "TEXT", nullable: true),
                    PromotionId = table.Column<int>(type: "INTEGER", nullable: true),
                    FamilleId = table.Column<int>(type: "INTEGER", nullable: true),
                    Photo = table.Column<byte[]>(type: "BLOB", nullable: true),
                    EleveParrainId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eleves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eleves_Eleves_EleveParrainId",
                        column: x => x.EleveParrainId,
                        principalTable: "Eleves",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Eleves_Familles_FamilleId",
                        column: x => x.FamilleId,
                        principalTable: "Familles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Eleves_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questionnaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EleveId = table.Column<int>(type: "INTEGER", nullable: false),
                    Provenance = table.Column<string>(type: "TEXT", nullable: false),
                    Astro = table.Column<string>(type: "TEXT", nullable: false),
                    Boisson = table.Column<string>(type: "TEXT", nullable: false),
                    Soiree = table.Column<string>(type: "TEXT", nullable: false),
                    Son = table.Column<string>(type: "TEXT", nullable: false),
                    Livre = table.Column<string>(type: "TEXT", nullable: false),
                    Film = table.Column<string>(type: "TEXT", nullable: false),
                    PasseTemps = table.Column<string>(type: "TEXT", nullable: false),
                    Defaut = table.Column<string>(type: "TEXT", nullable: false),
                    Qualite = table.Column<string>(type: "TEXT", nullable: false),
                    Relation = table.Column<string>(type: "TEXT", nullable: false),
                    Preference = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questionnaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questionnaires_Eleves_EleveId",
                        column: x => x.EleveId,
                        principalTable: "Eleves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voeux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EleveId = table.Column<int>(type: "INTEGER", nullable: false),
                    PromotionId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumVoeux = table.Column<int>(type: "INTEGER", nullable: false),
                    EleveChoisiId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voeux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voeux_Eleves_EleveChoisiId",
                        column: x => x.EleveChoisiId,
                        principalTable: "Eleves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voeux_Eleves_EleveId",
                        column: x => x.EleveId,
                        principalTable: "Eleves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Voeux_Promotions_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eleves_EleveParrainId",
                table: "Eleves",
                column: "EleveParrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Eleves_FamilleId",
                table: "Eleves",
                column: "FamilleId");

            migrationBuilder.CreateIndex(
                name: "IX_Eleves_PromotionId",
                table: "Eleves",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questionnaires_EleveId",
                table: "Questionnaires",
                column: "EleveId");

            migrationBuilder.CreateIndex(
                name: "IX_Voeux_EleveChoisiId",
                table: "Voeux",
                column: "EleveChoisiId");

            migrationBuilder.CreateIndex(
                name: "IX_Voeux_EleveId",
                table: "Voeux",
                column: "EleveId");

            migrationBuilder.CreateIndex(
                name: "IX_Voeux_PromotionId",
                table: "Voeux",
                column: "PromotionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questionnaires");

            migrationBuilder.DropTable(
                name: "Voeux");

            migrationBuilder.DropTable(
                name: "Eleves");

            migrationBuilder.DropTable(
                name: "Familles");

            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
