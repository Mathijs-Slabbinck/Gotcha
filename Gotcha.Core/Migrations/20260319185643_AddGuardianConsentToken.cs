using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gotcha.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddGuardianConsentToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuardianConsentToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuardianConsentToken",
                table: "AspNetUsers");
        }
    }
}
