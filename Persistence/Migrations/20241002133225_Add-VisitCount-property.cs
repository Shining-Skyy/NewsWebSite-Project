using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitCountproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(707),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(8419));

            migrationBuilder.AddColumn<int>(
                name: "VisitCount",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(8741),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(5358));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostFavorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(5227),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(2299));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 137, DateTimeKind.Local).AddTicks(5520),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(3927));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitCount",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(8419),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(707));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(5358),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(8741));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostFavorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(2299),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 138, DateTimeKind.Local).AddTicks(5227));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(3927),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 2, 6, 32, 25, 137, DateTimeKind.Local).AddTicks(5520));
        }
    }
}
