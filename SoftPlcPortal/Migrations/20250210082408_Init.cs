using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftPlcPortal.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlcConfigs",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    PlcPort = table.Column<int>(type: "INTEGER", nullable: false),
                    ApiPort = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlcConfigs", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "DataBlocks",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlcConfigKey = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataBlocks", x => x.Key);
                    table.ForeignKey(
                        name: "FK_DataBlocks_PlcConfigs_PlcConfigKey",
                        column: x => x.PlcConfigKey,
                        principalTable: "PlcConfigs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbFields",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataBlockKey = table.Column<Guid>(type: "TEXT", nullable: false),
                    ByteOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    BitOffset = table.Column<int>(type: "INTEGER", nullable: false),
                    DataType = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    StartValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbFields", x => x.Key);
                    table.ForeignKey(
                        name: "FK_DbFields_DataBlocks_DataBlockKey",
                        column: x => x.DataBlockKey,
                        principalTable: "DataBlocks",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataBlocks_Name",
                table: "DataBlocks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataBlocks_PlcConfigKey_Number",
                table: "DataBlocks",
                columns: new[] { "PlcConfigKey", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DbFields_DataBlockKey",
                table: "DbFields",
                column: "DataBlockKey");

            migrationBuilder.CreateIndex(
                name: "IX_PlcConfigs_Address_PlcPort",
                table: "PlcConfigs",
                columns: new[] { "Address", "PlcPort" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlcConfigs_Name",
                table: "PlcConfigs",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbFields");

            migrationBuilder.DropTable(
                name: "DataBlocks");

            migrationBuilder.DropTable(
                name: "PlcConfigs");
        }
    }
}
