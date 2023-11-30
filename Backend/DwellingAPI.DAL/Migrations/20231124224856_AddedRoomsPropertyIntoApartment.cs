using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DwellingAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomsPropertyIntoApartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rooms",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rooms",
                table: "Apartments");
        }
    }
}
