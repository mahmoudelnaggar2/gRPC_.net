using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GRPCCustomers.Server.Data.Migrations
{
    public partial class addcustomersentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityTown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    ReadingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readings_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Address1", "Address2", "Address3", "CityTown", "Country", "PostalCode", "StateProvince" },
                values: new object[] { 1, "123 Main Street", null, null, "Atlanta", null, "30303", "GA" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Address1", "Address2", "Address3", "CityTown", "Country", "PostalCode", "StateProvince" },
                values: new object[] { 2, "123 Side Street", null, null, "Atlanta", null, "30304", "GA" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "AddressId", "CompanyName", "Name", "PhoneNumber" },
                values: new object[] { 1, 1, null, "Shawn Wildermuth", "555-1212" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "AddressId", "CompanyName", "Name", "PhoneNumber" },
                values: new object[] { 2, 2, null, "Jake Smith", "(404) 555-1212" });

            migrationBuilder.InsertData(
                table: "Readings",
                columns: new[] { "Id", "CustomerId", "ReadingDate", "Value" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 1, 2, 21, 37, 9, 149, DateTimeKind.Local).AddTicks(7405), 1458.9000000000001 },
                    { 2, 1, new DateTime(2023, 1, 2, 21, 37, 9, 149, DateTimeKind.Local).AddTicks(7423), 18403.5 },
                    { 3, 2, new DateTime(2023, 1, 2, 21, 37, 9, 149, DateTimeKind.Local).AddTicks(7424), 0.0 },
                    { 4, 2, new DateTime(2023, 1, 2, 21, 37, 9, 149, DateTimeKind.Local).AddTicks(7424), 304.75 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_CustomerId",
                table: "Readings",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Readings");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
