using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SimpleBankAPI.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "Name" },
                values: new object[,]
                {
                    { new Guid("33983e90-750a-475d-b4f8-e0ff8800c6a5"), 100m, "Lazy Susan" },
                    { new Guid("6e3ba52b-dccc-4640-9d70-677f9c6e22ba"), 25000m, "Mary Poppins" },
                    { new Guid("747748e4-2647-4e16-a64c-47745fca51d3"), 5000m, "John Deere" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("33983e90-750a-475d-b4f8-e0ff8800c6a5"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("6e3ba52b-dccc-4640-9d70-677f9c6e22ba"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("747748e4-2647-4e16-a64c-47745fca51d3"));
        }
    }
}
