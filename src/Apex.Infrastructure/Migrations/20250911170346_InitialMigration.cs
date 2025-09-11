using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Apex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "domain");

            migrationBuilder.CreateTable(
                name: "drivers",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DriverNumber = table.Column<int>(type: "integer", nullable: false),
                    BroadcastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NameAcronym = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HeadshotUrl = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "meetings",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<int>(type: "integer", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OfficialName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryKey = table.Column<int>(type: "integer", maxLength: 3, nullable: false),
                    CountryName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CircuitKey = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    CircuitShortName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TeamColour = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeetingId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<int>(type: "integer", maxLength: 20, nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GmtOffset = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sessions_meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalSchema: "domain",
                        principalTable: "meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingestion_statuses",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    DataType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecordsCount = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingestion_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ingestion_statuses_sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "domain",
                        principalTable: "sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "laps",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    DriverId = table.Column<int>(type: "integer", nullable: false),
                    LapNumber = table.Column<int>(type: "integer", nullable: false),
                    DateStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LapDurationMs = table.Column<int>(type: "integer", nullable: true),
                    DurationSector1Ms = table.Column<int>(type: "integer", nullable: true),
                    DurationSector2Ms = table.Column<int>(type: "integer", nullable: true),
                    DurationSector3Ms = table.Column<int>(type: "integer", nullable: true),
                    I1Speed = table.Column<int>(type: "integer", nullable: true),
                    I2Speed = table.Column<int>(type: "integer", nullable: true),
                    FinishLineSpeed = table.Column<int>(type: "integer", nullable: true),
                    StSpeed = table.Column<int>(type: "integer", nullable: true),
                    IsPitOutLap = table.Column<bool>(type: "boolean", nullable: false),
                    SegmentsJson = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_laps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_laps_drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "domain",
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_laps_sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "domain",
                        principalTable: "sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pit_stops",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    DriverId = table.Column<int>(type: "integer", nullable: false),
                    LapNumber = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PitDuration = table.Column<decimal>(type: "numeric(5,3)", precision: 5, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pit_stops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pit_stops_drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "domain",
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pit_stops_sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "domain",
                        principalTable: "sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "race_controls",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LapNumber = table.Column<int>(type: "integer", nullable: true),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Flag = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    DriverId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_race_controls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_race_controls_drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "domain",
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_race_controls_sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "domain",
                        principalTable: "sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session_drivers",
                schema: "domain",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    DriverId = table.Column<int>(type: "integer", nullable: false),
                    TeamId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_drivers", x => new { x.SessionId, x.DriverId });
                    table.ForeignKey(
                        name: "FK_session_drivers_drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "domain",
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_session_drivers_sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "domain",
                        principalTable: "sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_session_drivers_teams_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "domain",
                        principalTable: "teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stints",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    DriverId = table.Column<int>(type: "integer", nullable: false),
                    StintNumber = table.Column<int>(type: "integer", nullable: false),
                    LapStart = table.Column<int>(type: "integer", nullable: false),
                    LapEnd = table.Column<int>(type: "integer", nullable: false),
                    TyreAgeAtStart = table.Column<int>(type: "integer", nullable: true),
                    Compound = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stints_drivers_DriverId",
                        column: x => x.DriverId,
                        principalSchema: "domain",
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stints_sessions_SessionId",
                        column: x => x.SessionId,
                        principalSchema: "domain",
                        principalTable: "sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_drivers_DriverNumber",
                schema: "domain",
                table: "drivers",
                column: "DriverNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ingestion_statuses_SessionId",
                schema: "domain",
                table: "ingestion_statuses",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_laps_DriverId",
                schema: "domain",
                table: "laps",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_laps_SessionId_DriverId",
                schema: "domain",
                table: "laps",
                columns: new[] { "SessionId", "DriverId" });

            migrationBuilder.CreateIndex(
                name: "IX_laps_SessionId_DriverId_LapNumber",
                schema: "domain",
                table: "laps",
                columns: new[] { "SessionId", "DriverId", "LapNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pit_stops_DriverId",
                schema: "domain",
                table: "pit_stops",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_pit_stops_SessionId",
                schema: "domain",
                table: "pit_stops",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_race_controls_DriverId",
                schema: "domain",
                table: "race_controls",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_race_controls_SessionId",
                schema: "domain",
                table: "race_controls",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_session_drivers_DriverId",
                schema: "domain",
                table: "session_drivers",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_session_drivers_TeamId",
                schema: "domain",
                table: "session_drivers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_MeetingId",
                schema: "domain",
                table: "sessions",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_stints_DriverId",
                schema: "domain",
                table: "stints",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_stints_SessionId",
                schema: "domain",
                table: "stints",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ingestion_statuses",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "laps",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "pit_stops",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "race_controls",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "session_drivers",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "stints",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "teams",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "drivers",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "sessions",
                schema: "domain");

            migrationBuilder.DropTable(
                name: "meetings",
                schema: "domain");
        }
    }
}
