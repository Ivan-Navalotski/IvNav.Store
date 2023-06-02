using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvNav.Store.Infrastructure.Migrations.IdentityDb
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedSetupPassword",
                schema: "identity",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedSetupPassword",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
