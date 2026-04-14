using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_board.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTypeAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Projects",
                newName: "ProjectType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectType",
                table: "Projects",
                newName: "Type");
        }
    }
}
