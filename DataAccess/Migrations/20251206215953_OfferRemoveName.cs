using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OfferRemoveName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aggrements_Offers_OfferId",
                table: "Aggrements");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Offers");

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "Aggrements",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Aggrements_Offers_OfferId",
                table: "Aggrements",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aggrements_Offers_OfferId",
                table: "Aggrements");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Offers",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfferId",
                table: "Aggrements",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Aggrements_Offers_OfferId",
                table: "Aggrements",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
