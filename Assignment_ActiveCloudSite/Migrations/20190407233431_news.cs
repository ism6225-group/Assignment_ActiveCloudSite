using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment_ActiveCloudSite.Migrations
{
    public partial class news : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    datetime = table.Column<string>(nullable: false),
                    headline = table.Column<string>(nullable: true),
                    source = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    summary = table.Column<string>(nullable: true),
                    related = table.Column<string>(nullable: true),
                    image = table.Column<string>(nullable: true),
                    symbol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.datetime);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
