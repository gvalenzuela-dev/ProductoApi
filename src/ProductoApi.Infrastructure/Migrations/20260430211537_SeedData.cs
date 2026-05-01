using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductoApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "This is a sample product.", "Sample Product", 9.99m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "This is another sample product.", "Another Product", 19.99m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "This is the third sample product.", "Third Product", 29.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
