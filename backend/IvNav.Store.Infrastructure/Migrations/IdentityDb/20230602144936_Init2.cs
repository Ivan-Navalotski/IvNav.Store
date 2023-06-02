using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvNav.Store.Infrastructure.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                schema: "identity",
                table: "UserExternalProviderLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "identity",
                table: "UserExternalProviderLinks");
        }
    }
}
