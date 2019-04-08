using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment_ActiveCloudSite.Migrations
{
    public partial class quotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    symbol = table.Column<string>(nullable: false),
                    companyName = table.Column<string>(nullable: true),
                    primaryExchange = table.Column<string>(nullable: true),
                    sector = table.Column<string>(nullable: true),
                    calculationPrice = table.Column<string>(nullable: true),
                    open = table.Column<float>(nullable: true),
                    openTime = table.Column<long>(nullable: true),
                    close = table.Column<float>(nullable: true),
                    closeTime = table.Column<long>(nullable: true),
                    high = table.Column<float>(nullable: true),
                    low = table.Column<float>(nullable: true),
                    latestPrice = table.Column<float>(nullable: true),
                    latestSource = table.Column<string>(nullable: true),
                    latestTime = table.Column<string>(nullable: true),
                    latestUpdate = table.Column<long>(nullable: true),
                    latestVolume = table.Column<long>(nullable: true),
                    iexRealtimePrice = table.Column<float>(nullable: true),
                    iexRealtimeSize = table.Column<long>(nullable: true),
                    iexLastUpdated = table.Column<string>(nullable: true),
                    delayedPrice = table.Column<float>(nullable: true),
                    delayedPriceTime = table.Column<long>(nullable: true),
                    extendedPrice = table.Column<float>(nullable: true),
                    extendedChange = table.Column<float>(nullable: true),
                    extendedChangePercent = table.Column<float>(nullable: true),
                    extendedPriceTime = table.Column<long>(nullable: true),
                    previousClose = table.Column<float>(nullable: true),
                    change = table.Column<float>(nullable: true),
                    changePercent = table.Column<float>(nullable: true),
                    iexMarketPercent = table.Column<float>(nullable: true),
                    iexVolume = table.Column<long>(nullable: true),
                    avgTotalVolume = table.Column<float>(nullable: true),
                    iexBidPrice = table.Column<float>(nullable: true),
                    iexBidSize = table.Column<long>(nullable: true),
                    iexAskPrice = table.Column<float>(nullable: true),
                    iexAskSize = table.Column<long>(nullable: true),
                    marketCap = table.Column<float>(nullable: true),
                    peRatio = table.Column<float>(nullable: true),
                    week52High = table.Column<float>(nullable: true),
                    week52Low = table.Column<float>(nullable: true),
                    ytdChange = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.symbol);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");
        }
    }
}
