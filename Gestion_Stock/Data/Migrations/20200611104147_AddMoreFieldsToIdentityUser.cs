using Microsoft.EntityFrameworkCore.Migrations;

namespace Gestion_Stock.Data.Migrations
{
    public partial class AddMoreFieldsToIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codePostal",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nom",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ville",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "codePostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "nom",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
