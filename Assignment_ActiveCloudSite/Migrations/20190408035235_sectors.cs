using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment_ActiveCloudSite.Migrations
{
    public partial class sectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    name = table.Column<string>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    performance = table.Column<float>(nullable: true),
                    lastUppdated = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sectors");
        }
    }
}
