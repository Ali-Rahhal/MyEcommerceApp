using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyRecordsBySeedingTheTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "Techville", "Tech Solutions", "123-456-7890", "12345", "TS", "123 Tech Lane" },
                    { 2, "Businesstown", "Business Corp", "987-654-3210", "67890", "BC", "456 Business Rd" },
                    { 3, "Enterprisecity", "Enterprise Inc", "555-555-5555", "11223", "EI", "789 Enterprise Ave" },
                    { 4, "Globaltown", "Global Industries", "444-444-4444", "44556", "GI", "101 Global St" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
