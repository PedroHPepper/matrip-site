using Microsoft.EntityFrameworkCore.Migrations;

namespace Matrip.Web.Migrations
{
    public partial class b2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ma01password",
                table: "ma01user",
                newName: "ma01PasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "ma01FullName",
                table: "ma01user",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ma01FullName",
                table: "ma01user");

            migrationBuilder.RenameColumn(
                name: "ma01PasswordHash",
                table: "ma01user",
                newName: "ma01password");
        }
    }
}
