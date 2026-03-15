using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.Data.Migrations
{
    public partial class AddOwnerIdToTodoLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "TodoLists",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TodoLists");
        }
    }
}
