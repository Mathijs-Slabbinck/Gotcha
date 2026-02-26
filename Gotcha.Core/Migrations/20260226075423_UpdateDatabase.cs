using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gotcha.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChaosTimer",
                table: "Rules",
                newName: "ChaosTimerMin");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignmentExpirationDate",
                table: "TargetAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomRules",
                table: "Rules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ChaosTimerMax",
                table: "Rules",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "ShowGender",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowHunter",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLivingPlayerCount",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLivingPlayerNames",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowLivingPlayerNamesToDeath",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpectator",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AssignmentExpirationDate",
                table: "TargetAssignments");

            migrationBuilder.DropColumn(
                name: "ChaosTimerMax",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "ShowGender",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "ShowHunter",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "ShowLivingPlayerCount",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "ShowLivingPlayerNames",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "ShowLivingPlayerNamesToDeath",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "IsSpectator",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "ChaosTimerMin",
                table: "Rules",
                newName: "ChaosTimer");

            migrationBuilder.AlterColumn<string>(
                name: "CustomRules",
                table: "Rules",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
