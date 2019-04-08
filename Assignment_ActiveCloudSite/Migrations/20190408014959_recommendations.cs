using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment_ActiveCloudSite.Migrations
{
    public partial class recommendations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    symbol = table.Column<string>(nullable: false),
                    consensusEndDate = table.Column<float>(nullable: true),
                    consensusStartDate = table.Column<float>(nullable: true),
                    corporateActionsAppliedDate = table.Column<float>(nullable: true),
                    ratingBuy = table.Column<float>(nullable: true),
                    ratingHold = table.Column<float>(nullable: true),
                    ratingNone = table.Column<float>(nullable: true),
                    ratingOverweight = table.Column<float>(nullable: true),
                    ratingScaleMark = table.Column<float>(nullable: true),
                    ratingSell = table.Column<float>(nullable: true),
                    ratingUnderweight = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.symbol);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
