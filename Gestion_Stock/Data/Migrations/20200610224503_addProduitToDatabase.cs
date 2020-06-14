using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestion_Stock.Data.Migrations
{
    public partial class addProduitToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produit",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(nullable: false),
                    CategorieId = table.Column<int>(nullable: false),
                    marque = table.Column<string>(nullable: false),
                    qte = table.Column<int>(nullable: false),
                    dateStock = table.Column<DateTime>(nullable: false),
                    livre = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produit", x => x.id);
                    table.ForeignKey(
                        name: "FK_Produit_Categorie_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categorie",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produit_CategorieId",
                table: "Produit",
                column: "CategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produit");
        }
    }
}
