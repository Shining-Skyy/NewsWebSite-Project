using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations.IdentityDataBase
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                schema: "identity",
                table: "UserTokens");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "UserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "identity",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                schema: "identity",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                schema: "identity",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "identity",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                schema: "identity",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider" });
        }
    }
}
