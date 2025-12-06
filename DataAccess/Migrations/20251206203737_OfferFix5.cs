using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OfferFix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Aggrements_OfferId",
                table: "Aggrements",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aggrements_Offers_OfferId",
                table: "Aggrements",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aggrements_Offers_OfferId",
                table: "Aggrements");

            migrationBuilder.DropIndex(
                name: "IX_Aggrements_OfferId",
                table: "Aggrements");
        }
    }
}
