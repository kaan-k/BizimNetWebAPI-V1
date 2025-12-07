using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Duties_AssignedEmployeeId",
                table: "Duties",
                column: "AssignedEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_BusinessUsers_AssignedEmployeeId",
                table: "Duties",
                column: "AssignedEmployeeId",
                principalTable: "BusinessUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_BusinessUsers_AssignedEmployeeId",
                table: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_Duties_AssignedEmployeeId",
                table: "Duties");
        }
    }
}
