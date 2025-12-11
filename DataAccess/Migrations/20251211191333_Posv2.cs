using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Posv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_StockGroup_StockGroupId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockGroup",
                table: "StockGroup");

            migrationBuilder.RenameTable(
                name: "StockGroup",
                newName: "StockGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockGroups",
                table: "StockGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_StockGroups_StockGroupId",
                table: "Stocks",
                column: "StockGroupId",
                principalTable: "StockGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_StockGroups_StockGroupId",
                table: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockGroups",
                table: "StockGroups");

            migrationBuilder.RenameTable(
                name: "StockGroups",
                newName: "StockGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockGroup",
                table: "StockGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_StockGroup_StockGroupId",
                table: "Stocks",
                column: "StockGroupId",
                principalTable: "StockGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
