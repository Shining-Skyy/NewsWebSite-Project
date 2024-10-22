using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableforComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 566, DateTimeKind.Local).AddTicks(4412),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 264, DateTimeKind.Local).AddTicks(2942));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 567, DateTimeKind.Local).AddTicks(3419),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 265, DateTimeKind.Local).AddTicks(1248));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostFavorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 566, DateTimeKind.Local).AddTicks(9736),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 264, DateTimeKind.Local).AddTicks(7717));

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertTime",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 565, DateTimeKind.Local).AddTicks(7878));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemove",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemoveTime",
                table: "Comments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Comments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 565, DateTimeKind.Local).AddTicks(2483),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 263, DateTimeKind.Local).AddTicks(7528));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertTime",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsRemove",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "RemoveTime",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Comments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 264, DateTimeKind.Local).AddTicks(2942),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 566, DateTimeKind.Local).AddTicks(4412));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 265, DateTimeKind.Local).AddTicks(1248),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 567, DateTimeKind.Local).AddTicks(3419));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostFavorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 264, DateTimeKind.Local).AddTicks(7717),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 566, DateTimeKind.Local).AddTicks(9736));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 18, 9, 54, 34, 263, DateTimeKind.Local).AddTicks(7528),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 18, 9, 58, 39, 565, DateTimeKind.Local).AddTicks(2483));
        }
    }
}
