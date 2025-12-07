using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PosOptimizer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PosRatios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PosName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Installment = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosRatios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PosRatios");
        }
    }
}
