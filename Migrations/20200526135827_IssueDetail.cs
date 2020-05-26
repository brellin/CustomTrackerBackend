using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomTracker.Migrations
{
    public partial class IssueDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "issues",
                nullable : true,
                oldClrType : typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "detail",
                table: "issues",
                nullable : true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "detail",
                table: "issues");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "issues",
                type: "text",
                nullable : false,
                oldClrType : typeof(string),
                oldNullable : true);
        }
    }
}
