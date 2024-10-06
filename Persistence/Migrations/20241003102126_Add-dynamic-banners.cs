using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adddynamicbanners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 310, DateTimeKind.Local).AddTicks(3788),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(707));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 311, DateTimeKind.Local).AddTicks(1433),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(8741));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostFavorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 310, DateTimeKind.Local).AddTicks(8235),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(5227));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 309, DateTimeKind.Local).AddTicks(5829),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 137, DateTimeKind.Local).AddTicks(5520));

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(707),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 310, DateTimeKind.Local).AddTicks(3788));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(8741),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 311, DateTimeKind.Local).AddTicks(1433));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostFavorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(5227),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 310, DateTimeKind.Local).AddTicks(8235));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 137, DateTimeKind.Local).AddTicks(5520),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 3, 3, 21, 26, 309, DateTimeKind.Local).AddTicks(5829));
        }
    }
}
