using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomTrackerBackend.Migrations
{
    public partial class GroupsNameUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "issues",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "groups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_groups_name",
                table: "groups",
                column: "name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_groups_name",
                table: "groups");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "issues",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "groups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
