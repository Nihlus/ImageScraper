﻿// <auto-generated />

using Microsoft.EntityFrameworkCore.Migrations;

namespace Argus.Coordinator.Migrations
{
    public partial class RenameImageToLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "report_image",
                table: "service_status_reports",
                newName: "report_link");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "report_link",
                table: "service_status_reports",
                newName: "report_image");
        }
    }
}