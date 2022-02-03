﻿// <auto-generated />

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CS1591

namespace Argus.Coordinator.Migrations
{
    public partial class IndexTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_service_status_reports_timestamp",
                table: "service_status_reports",
                column: "timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_service_status_reports_timestamp",
                table: "service_status_reports");
        }
    }
}
