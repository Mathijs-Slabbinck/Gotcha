using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gotcha.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attackers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Referer = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InvalidInput = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MacAddress = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attackers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAssassin = table.Column<bool>(type: "bit", nullable: false),
                    ShowPlayerImages = table.Column<bool>(type: "bit", nullable: false),
                    EnforcePlayerImages = table.Column<bool>(type: "bit", nullable: false),
                    ShowRealNames = table.Column<bool>(type: "bit", nullable: false),
                    ShowUsernames = table.Column<bool>(type: "bit", nullable: false),
                    IsTimed = table.Column<bool>(type: "bit", nullable: false),
                    TargetTimeOut = table.Column<long>(type: "bigint", nullable: false),
                    CustomRules = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CustomKillMethods = table.Column<bool>(type: "bit", nullable: false),
                    KillMethods = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsChaos = table.Column<bool>(type: "bit", nullable: false),
                    ChaosTimer = table.Column<long>(type: "bigint", nullable: false),
                    KillConfirmationTimer = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProfileImageSource = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    LogSubType = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExtraInfo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AttackerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Attackers_AttackerId",
                        column: x => x.AttackerId,
                        principalTable: "Attackers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasStarted = table.Column<bool>(type: "bit", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AdminIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxPlayers = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Rules_RulesId",
                        column: x => x.RulesId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VipSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssassinModeUnlocked = table.Column<bool>(type: "bit", nullable: false),
                    ChaosModeUnlocked = table.Column<bool>(type: "bit", nullable: false),
                    TimedKillsUnlocked = table.Column<bool>(type: "bit", nullable: false),
                    CustomKillMethodsUnlocked = table.Column<bool>(type: "bit", nullable: false),
                    MaxLobbySize = table.Column<int>(type: "int", nullable: false),
                    UserPlan = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VipSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VipSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProfileImageSource = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Players_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KillerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VictimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Moment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weapon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    KillMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TimeSinceAssignedTarget = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kills_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kills_Players_KillerId",
                        column: x => x.KillerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kills_Players_VictimId",
                        column: x => x.VictimId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HunterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignmentFinished = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KillId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Weapon = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AssignmentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TargetAssignments_Kills_KillId",
                        column: x => x.KillId,
                        principalTable: "Kills",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TargetAssignments_Players_HunterId",
                        column: x => x.HunterId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetAssignments_Players_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_RulesId",
                table: "Games",
                column: "RulesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kills_GameId",
                table: "Kills",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Kills_KillerId",
                table: "Kills",
                column: "KillerId");

            migrationBuilder.CreateIndex(
                name: "IX_Kills_VictimId",
                table: "Kills",
                column: "VictimId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_AttackerId",
                table: "Logs",
                column: "AttackerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetAssignments_HunterId",
                table: "TargetAssignments",
                column: "HunterId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetAssignments_KillId",
                table: "TargetAssignments",
                column: "KillId",
                unique: true,
                filter: "[KillId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TargetAssignments_TargetId",
                table: "TargetAssignments",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_VipSettings_UserId",
                table: "VipSettings",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "TargetAssignments");

            migrationBuilder.DropTable(
                name: "VipSettings");

            migrationBuilder.DropTable(
                name: "Attackers");

            migrationBuilder.DropTable(
                name: "Kills");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rules");
        }
    }
}
