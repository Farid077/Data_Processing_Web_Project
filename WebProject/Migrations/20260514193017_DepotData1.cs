using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebProject.Migrations
{
    /// <inheritdoc />
    public partial class DepotData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Departments_DepartmentId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Roles_DepartmentId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "DepotData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SN = table.Column<int>(type: "integer", nullable: false),
                    DQN = table.Column<string>(type: "text", nullable: false),
                    EyNom = table.Column<string>(type: "text", nullable: false),
                    DVR = table.Column<string>(type: "text", nullable: false),
                    CD = table.Column<string>(type: "text", nullable: true),
                    QapiR = table.Column<string>(type: "text", nullable: true),
                    SayKam = table.Column<string>(type: "text", nullable: true),
                    HDDSt = table.Column<string>(type: "text", nullable: true),
                    HDDHc = table.Column<string>(type: "text", nullable: true),
                    HDDSM = table.Column<string>(type: "text", nullable: true),
                    DVRSt = table.Column<string>(type: "text", nullable: true),
                    Kam = table.Column<string>(type: "text", nullable: true),
                    KamSt = table.Column<string>(type: "text", nullable: true),
                    KamNom = table.Column<string>(type: "text", nullable: true),
                    SalMon = table.Column<string>(type: "text", nullable: true),
                    DaySes = table.Column<string>(type: "text", nullable: true),
                    SurMik = table.Column<string>(type: "text", nullable: true),
                    Trafared = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ConfirmedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ApproverId = table.Column<string>(type: "character varying(16)", nullable: true),
                    Depot = table.Column<int>(type: "integer", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepotData_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepotData_ApproverId",
                table: "DepotData",
                column: "ApproverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepotData");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Roles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "text", nullable: false, defaultValue: "Dashboard"),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Note = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, defaultValue: ""),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "Pending"),
                    SubCategory = table.Column<string>(type: "text", nullable: false, defaultValue: "Read_Write"),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Username = table.Column<string>(type: "character varying(16)", nullable: true),
                    Username1 = table.Column<string>(type: "character varying(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issues_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username");
                    table.ForeignKey(
                        name: "FK_Issues_Users_Username1",
                        column: x => x.Username1,
                        principalTable: "Users",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DepartmentId",
                table: "Roles",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Username",
                table: "Issues",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Issues_Username1",
                table: "Issues",
                column: "Username1");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Departments_DepartmentId",
                table: "Roles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
