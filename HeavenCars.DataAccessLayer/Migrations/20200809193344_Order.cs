using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeavenCars.DataAccessLayer.Migrations
{
    public partial class Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    PickUpDate = table.Column<DateTime>(nullable: false),
                    PickUpTime = table.Column<string>(nullable: true),
                    DropOffTime = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<double>(nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ReservationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
