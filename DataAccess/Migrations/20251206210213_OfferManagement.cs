using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OfferManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferItem_Offers_OfferId",
                table: "OfferItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItem_Stocks_StockId",
                table: "OfferItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferItem",
                table: "OfferItem");

            migrationBuilder.RenameTable(
                name: "OfferItem",
                newName: "OfferItems");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItem_StockId",
                table: "OfferItems",
                newName: "IX_OfferItems_StockId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItem_OfferId",
                table: "OfferItems",
                newName: "IX_OfferItems_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferItems",
                table: "OfferItems",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Offers_OfferId",
                table: "OfferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferItems_Stocks_StockId",
                table: "OfferItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferItems",
                table: "OfferItems");

            migrationBuilder.RenameTable(
                name: "OfferItems",
                newName: "OfferItem");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItems_StockId",
                table: "OfferItem",
                newName: "IX_OfferItem_StockId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferItems_OfferId",
                table: "OfferItem",
                newName: "IX_OfferItem_OfferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferItem",
                table: "OfferItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItem_Offers_OfferId",
                table: "OfferItem",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferItem_Stocks_StockId",
                table: "OfferItem",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
