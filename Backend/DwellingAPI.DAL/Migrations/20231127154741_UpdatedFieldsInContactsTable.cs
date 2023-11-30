using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DwellingAPI.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedFieldsInContactsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Provider",
                table: "Contacts",
                newName: "ContactOptionValue");

            migrationBuilder.RenameColumn(
                name: "MobilePhone",
                table: "Contacts",
                newName: "ContactOptionName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactOptionValue",
                table: "Contacts",
                newName: "Provider");

            migrationBuilder.RenameColumn(
                name: "ContactOptionName",
                table: "Contacts",
                newName: "MobilePhone");
        }
    }
}
