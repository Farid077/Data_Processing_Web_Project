using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProject.Migrations
{
    /// <inheritdoc />
    public partial class Depots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepotData_Users_ApproverId",
                table: "DepotData");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "DepotData",
                newName: "Qeyd");

            migrationBuilder.RenameColumn(
                name: "KamSt",
                table: "DepotData",
                newName: "KamV");

            migrationBuilder.RenameColumn(
                name: "HDDSt",
                table: "DepotData",
                newName: "HDDV");

            migrationBuilder.RenameColumn(
                name: "HDDHc",
                table: "DepotData",
                newName: "HDDH");

            migrationBuilder.RenameColumn(
                name: "DVRSt",
                table: "DepotData",
                newName: "DVRV");

            migrationBuilder.RenameColumn(
                name: "ApproverId",
                table: "DepotData",
                newName: "ConfirmerId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotData_ApproverId",
                table: "DepotData",
                newName: "IX_DepotData_ConfirmerId");

            migrationBuilder.AlterColumn<int>(
                name: "SN",
                table: "DepotData",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "EyNom",
                table: "DepotData",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DVR",
                table: "DepotData",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DQN",
                table: "DepotData",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_DepotData_Users_ConfirmerId",
                table: "DepotData",
                column: "ConfirmerId",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepotData_Users_ConfirmerId",
                table: "DepotData");

            migrationBuilder.RenameColumn(
                name: "Qeyd",
                table: "DepotData",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "KamV",
                table: "DepotData",
                newName: "KamSt");

            migrationBuilder.RenameColumn(
                name: "HDDV",
                table: "DepotData",
                newName: "HDDSt");

            migrationBuilder.RenameColumn(
                name: "HDDH",
                table: "DepotData",
                newName: "HDDHc");

            migrationBuilder.RenameColumn(
                name: "DVRV",
                table: "DepotData",
                newName: "DVRSt");

            migrationBuilder.RenameColumn(
                name: "ConfirmerId",
                table: "DepotData",
                newName: "ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotData_ConfirmerId",
                table: "DepotData",
                newName: "IX_DepotData_ApproverId");

            migrationBuilder.AlterColumn<int>(
                name: "SN",
                table: "DepotData",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EyNom",
                table: "DepotData",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DVR",
                table: "DepotData",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DQN",
                table: "DepotData",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotData_Users_ApproverId",
                table: "DepotData",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
