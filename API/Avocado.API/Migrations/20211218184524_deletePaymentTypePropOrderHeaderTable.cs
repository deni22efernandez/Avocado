using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.API.Migrations
{
    public partial class deletePaymentTypePropOrderHeaderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_PaymentTypes_PaymentTypeId",
                table: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_PaymentTypeId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "OrderHeaders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_PaymentTypeId",
                table: "OrderHeaders",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_PaymentTypes_PaymentTypeId",
                table: "OrderHeaders",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
