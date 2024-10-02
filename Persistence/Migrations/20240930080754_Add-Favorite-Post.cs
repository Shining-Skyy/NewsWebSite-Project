using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(8419),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 2, 20, 57, 0, 569, DateTimeKind.Local).AddTicks(481));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(5358),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 2, 20, 57, 0, 569, DateTimeKind.Local).AddTicks(4791));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(3927),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 2, 20, 57, 0, 568, DateTimeKind.Local).AddTicks(5297));

            migrationBuilder.CreateTable(
                name: "PostFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(2299)),
                    IsRemove = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostFavorites_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostFavorites_PostId",
                table: "PostFavorites",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostFavorites");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 2, 20, 57, 0, 569, DateTimeKind.Local).AddTicks(481),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(8419));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "PostImage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 2, 20, 57, 0, 569, DateTimeKind.Local).AddTicks(4791),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 861, DateTimeKind.Local).AddTicks(5358));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "CategoryTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 2, 20, 57, 0, 568, DateTimeKind.Local).AddTicks(5297),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 30, 1, 7, 53, 860, DateTimeKind.Local).AddTicks(3927));
        }
    }
}
