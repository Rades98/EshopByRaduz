using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockUnits_StockItemId",
                table: "StockUnits");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "StockUnits",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_StockUnit_StockItemId_SerialNumber",
                table: "StockUnits",
                columns: new[] { "StockItemId", "SerialNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockUnit_StockItemId_SerialNumber",
                table: "StockUnits");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "StockUnits",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_StockUnits_StockItemId",
                table: "StockUnits",
                column: "StockItemId");
        }
    }
}
