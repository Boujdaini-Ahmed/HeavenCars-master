using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeavenCars.DataAccessLayer.Migrations
{
    public partial class ApprovalTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingApproval",
                table: "BookingVehicules",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingApprovalDate",
                table: "BookingVehicules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingApproval",
                table: "BookingVehicules");

            migrationBuilder.DropColumn(
                name: "BookingApprovalDate",
                table: "BookingVehicules");
        }
    }
}
