using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitiesManager.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "CityID",
                keyValue: new Guid("858462ef-5587-48d5-8db3-392938699f42"),
                column: "CountryID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "CityID",
                keyValue: new Guid("bf160cfd-e693-4c6a-9417-037b4501ec9b"),
                column: "CountryID",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Cities");
        }
    }
}
