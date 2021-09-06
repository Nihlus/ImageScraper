﻿// <auto-generated/>

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CS1591

namespace Argus.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APIKeys",
                columns: table => new
                {
                    ID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Key = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APIKeys", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APIKeys_ID",
                table: "APIKeys",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_APIKeys_Key",
                table: "APIKeys",
                column: "Key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APIKeys");
        }
    }
}
