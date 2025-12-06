using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BillingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Stocks_StockId",
                table: "OfferItems");

            migrationBuilder.DropIndex(
                name: "IX_OfferItems_StockId",
                table: "OfferItems");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "OfferItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "OfferItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferItems_StockId",
                table: "OfferItems",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItems_Stocks_StockId",
                table: "OfferItems",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
