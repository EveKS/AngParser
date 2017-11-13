using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AngParser.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScaningUriModels",
                columns: table => new
                {
                    ScaningUriModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaningUriModels", x => x.ScaningUriModelId);
                    table.ForeignKey(
                        name: "FK_ScaningUriModels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParsingEmailModels",
                columns: table => new
                {
                    ParsingEmailModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScaningUriModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Sended = table.Column<bool>(type: "bit", nullable: false),
                    UriString = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsingEmailModels", x => x.ParsingEmailModelId);
                    table.ForeignKey(
                        name: "FK_ParsingEmailModels_ScaningUriModels_ScaningUriModelId",
                        column: x => x.ScaningUriModelId,
                        principalTable: "ScaningUriModels",
                        principalColumn: "ScaningUriModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParsingEmailModels_ScaningUriModelId",
                table: "ParsingEmailModels",
                column: "ScaningUriModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaningUriModels_UserId",
                table: "ScaningUriModels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParsingEmailModels");

            migrationBuilder.DropTable(
                name: "ScaningUriModels");
        }
    }
}
