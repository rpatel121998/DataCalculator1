using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataCalculatorWebAppServer.Migrations
{
    public partial class FileData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileSendData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSendData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileStorageData",
                columns: table => new
                {
                    FileSendDataId = table.Column<int>(type: "int", nullable: false),
                    FileUri = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorageData", x => x.FileSendDataId);
                    table.ForeignKey(
                        name: "FK_FileStorageData_FileSendData_FileSendDataId",
                        column: x => x.FileSendDataId,
                        principalTable: "FileSendData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStorageData");

            migrationBuilder.DropTable(
                name: "FileSendData");
        }
    }
}
