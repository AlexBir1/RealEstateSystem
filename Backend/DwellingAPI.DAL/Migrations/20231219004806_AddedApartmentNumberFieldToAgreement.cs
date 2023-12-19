using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DwellingAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedApartmentNumberFieldToAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "Agreements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Agreements");
        }
    }
}
