using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IssueTracker.Migrations.IssueTracker
{
    public partial class AddAssigneesToTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssigneeId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssigneeId",
                table: "Tickets",
                column: "AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AssigneeId",
                table: "Tickets",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_AssigneeId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AssigneeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Tickets");
        }
    }
}
