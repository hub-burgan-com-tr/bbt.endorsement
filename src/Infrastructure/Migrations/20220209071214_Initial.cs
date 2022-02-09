using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Approvals",
                columns: table => new
                {
                    ApprovalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovalTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvals", x => x.ApprovalId);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    ApprovalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.ApprovalId);
                    table.ForeignKey(
                        name: "FK_References_Approvals_ApprovalId1",
                        column: x => x.ApprovalId1,
                        principalTable: "Approvals",
                        principalColumn: "ApprovalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Callbacks",
                columns: table => new
                {
                    CallbackId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstanceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Callbacks", x => x.CallbackId);
                    table.ForeignKey(
                        name: "FK_Callbacks_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "ApprovalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Callbacks_ReferenceId",
                table: "Callbacks",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_References_ApprovalId1",
                table: "References",
                column: "ApprovalId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Callbacks");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "Approvals");
        }
    }
}
