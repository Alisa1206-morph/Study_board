using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_board.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChecklistProjectRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistImage_Checklists_ChecklistId",
                table: "ChecklistImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChecklistImage",
                table: "ChecklistImage");

            migrationBuilder.DropColumn(
                name: "Projects",
                table: "Checklists");

            migrationBuilder.DropColumn(
                name: "IsMainImage",
                table: "ChecklistImage");

            migrationBuilder.RenameTable(
                name: "ChecklistImage",
                newName: "ChecklistImages");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistImage_ChecklistId",
                table: "ChecklistImages",
                newName: "IX_ChecklistImages_ChecklistId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Checklists",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Checklists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChecklistImages",
                table: "ChecklistImages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChecklistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StudyPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ChecklistId",
                table: "Projects",
                column: "ChecklistId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistImages_Checklists_ChecklistId",
                table: "ChecklistImages",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistImages_Checklists_ChecklistId",
                table: "ChecklistImages");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChecklistImages",
                table: "ChecklistImages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Checklists");

            migrationBuilder.RenameTable(
                name: "ChecklistImages",
                newName: "ChecklistImage");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistImages_ChecklistId",
                table: "ChecklistImage",
                newName: "IX_ChecklistImage_ChecklistId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Checklists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Projects",
                table: "Checklists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainImage",
                table: "ChecklistImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChecklistImage",
                table: "ChecklistImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistImage_Checklists_ChecklistId",
                table: "ChecklistImage",
                column: "ChecklistId",
                principalTable: "Checklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
