using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XtraBlogWebsite.Migrations
{
    public partial class NewLInkedin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Youtube",
                table: "Settings",
                newName: "Linkedin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Linkedin",
                table: "Settings",
                newName: "Youtube");
        }
    }
}
