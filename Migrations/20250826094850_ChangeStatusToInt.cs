using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardAPI.Migrations
{
    public partial class ChangeStatusToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add a temporary column
            migrationBuilder.AddColumn<int>(
                name: "StatusInt",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            // 2. Copy old string values to int
            migrationBuilder.Sql(@"
                UPDATE Tasks
                SET StatusInt = CASE Status
                    WHEN 'ToDo' THEN 0
                    WHEN 'InProgress' THEN 1
                    WHEN 'Completed' THEN 2
                    ELSE 0
                END
            ");

            // 3. Drop old string column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            // 4. Rename temporary column to 'Status'
            migrationBuilder.RenameColumn(
                name: "StatusInt",
                table: "Tasks",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Add back string column
            migrationBuilder.AddColumn<string>(
                name: "StatusString",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // 2. Convert int back to string
            migrationBuilder.Sql(@"
                UPDATE Tasks
                SET StatusString = CASE Status
                    WHEN 0 THEN 'ToDo'
                    WHEN 1 THEN 'InProgress'
                    WHEN 2 THEN 'Completed'
                    ELSE 'ToDo'
                END
            ");

            // 3. Drop int column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            // 4. Rename string column to 'Status'
            migrationBuilder.RenameColumn(
                name: "StatusString",
                table: "Tasks",
                newName: "Status");
        }
    }
}
