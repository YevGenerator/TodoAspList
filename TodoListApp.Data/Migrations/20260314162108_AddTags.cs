using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.Data.Migrations
{
    public partial class AddTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoTagEntityTodoTaskEntity",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTagEntityTodoTaskEntity", x => new { x.TagsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_TodoTagEntityTodoTaskEntity_TodoTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TodoTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoTagEntityTodoTaskEntity_TodoTasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "TodoTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoTagEntityTodoTaskEntity_TasksId",
                table: "TodoTagEntityTodoTaskEntity",
                column: "TasksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTagEntityTodoTaskEntity");

            migrationBuilder.DropTable(
                name: "TodoTags");
        }
    }
}
