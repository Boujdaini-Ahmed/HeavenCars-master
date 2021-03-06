﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeavenCars.DataAccessLayer.Migrations
{
    public partial class Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
