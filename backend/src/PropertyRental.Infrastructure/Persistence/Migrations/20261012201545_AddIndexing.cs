using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyRental.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_PropertyId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_RentalApplications_UserId",
                table: "RentalApplications");

            migrationBuilder.CreateIndex(
                name: "IX_Units_PropertyId_UnitNumber",
                table: "Units",
                columns: new[] { "PropertyId", "UnitNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalApplications_UserId_Status_CreatedAt_Id",
                table: "RentalApplications",
                columns: new[] { "UserId", "Status", "CreatedAt", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Name_Address",
                table: "Properties",
                columns: new[] { "Name", "Address" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_PropertyId_UnitNumber",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_RentalApplications_UserId_Status_CreatedAt_Id",
                table: "RentalApplications");

            migrationBuilder.DropIndex(
                name: "IX_Properties_Name_Address",
                table: "Properties");

            migrationBuilder.CreateIndex(
                name: "IX_Units_PropertyId",
                table: "Units",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalApplications_UserId",
                table: "RentalApplications",
                column: "UserId");
        }
    }
}
