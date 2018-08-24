using Microsoft.EntityFrameworkCore.Migrations;

namespace DbLogger.Core.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppLogItems",
                table: "AppLogItems");

            migrationBuilder.RenameTable(
                name: "AppLogItems",
                newName: "AppLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppLogs",
                table: "AppLogs",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppLogs",
                table: "AppLogs");

            migrationBuilder.RenameTable(
                name: "AppLogs",
                newName: "AppLogItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppLogItems",
                table: "AppLogItems",
                column: "Id");
        }
    }
}
