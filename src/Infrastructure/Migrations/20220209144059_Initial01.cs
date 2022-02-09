using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_References_Approvals_ApprovalId1",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_References_ApprovalId1",
                table: "References");

            migrationBuilder.DropColumn(
                name: "ApprovalId1",
                table: "References");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalTitle",
                table: "Approvals",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Approvals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_References_Approvals_ApprovalId",
                table: "References",
                column: "ApprovalId",
                principalTable: "Approvals",
                principalColumn: "ApprovalId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_References_Approvals_ApprovalId",
                table: "References");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "Approvals");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalId1",
                table: "References",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovalTitle",
                table: "Approvals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.CreateIndex(
                name: "IX_References_ApprovalId1",
                table: "References",
                column: "ApprovalId1");

            migrationBuilder.AddForeignKey(
                name: "FK_References_Approvals_ApprovalId1",
                table: "References",
                column: "ApprovalId1",
                principalTable: "Approvals",
                principalColumn: "ApprovalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
