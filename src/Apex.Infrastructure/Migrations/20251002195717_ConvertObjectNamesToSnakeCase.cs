using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertObjectNamesToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingestion_statuses_sessions_SessionId",
                schema: "domain",
                table: "ingestion_statuses");

            migrationBuilder.DropForeignKey(
                name: "FK_laps_drivers_DriverId",
                schema: "domain",
                table: "laps");

            migrationBuilder.DropForeignKey(
                name: "FK_laps_sessions_SessionId",
                schema: "domain",
                table: "laps");

            migrationBuilder.DropForeignKey(
                name: "FK_pit_stops_drivers_DriverId",
                schema: "domain",
                table: "pit_stops");

            migrationBuilder.DropForeignKey(
                name: "FK_pit_stops_sessions_SessionId",
                schema: "domain",
                table: "pit_stops");

            migrationBuilder.DropForeignKey(
                name: "FK_race_controls_drivers_DriverId",
                schema: "domain",
                table: "race_controls");

            migrationBuilder.DropForeignKey(
                name: "FK_race_controls_sessions_SessionId",
                schema: "domain",
                table: "race_controls");

            migrationBuilder.DropForeignKey(
                name: "FK_session_drivers_drivers_DriverId",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_session_drivers_sessions_SessionId",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_session_drivers_teams_TeamId",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_sessions_meetings_MeetingId",
                schema: "domain",
                table: "sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_stints_drivers_DriverId",
                schema: "domain",
                table: "stints");

            migrationBuilder.DropForeignKey(
                name: "FK_stints_sessions_SessionId",
                schema: "domain",
                table: "stints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teams",
                schema: "domain",
                table: "teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stints",
                schema: "domain",
                table: "stints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sessions",
                schema: "domain",
                table: "sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_session_drivers",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_race_controls",
                schema: "domain",
                table: "race_controls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pit_stops",
                schema: "domain",
                table: "pit_stops");

            migrationBuilder.DropPrimaryKey(
                name: "PK_meetings",
                schema: "domain",
                table: "meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_laps",
                schema: "domain",
                table: "laps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ingestion_statuses",
                schema: "domain",
                table: "ingestion_statuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_drivers",
                schema: "domain",
                table: "drivers");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "domain",
                table: "teams",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Key",
                schema: "domain",
                table: "teams",
                newName: "key");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "teams",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "domain",
                table: "teams",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeamColour",
                schema: "domain",
                table: "teams",
                newName: "team_colour");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "domain",
                table: "teams",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Compound",
                schema: "domain",
                table: "stints",
                newName: "compound");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "stints",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TyreAgeAtStart",
                schema: "domain",
                table: "stints",
                newName: "tyre_age_at_start");

            migrationBuilder.RenameColumn(
                name: "StintNumber",
                schema: "domain",
                table: "stints",
                newName: "stint_number");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "domain",
                table: "stints",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "LapStart",
                schema: "domain",
                table: "stints",
                newName: "lap_start");

            migrationBuilder.RenameColumn(
                name: "LapEnd",
                schema: "domain",
                table: "stints",
                newName: "lap_end");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                schema: "domain",
                table: "stints",
                newName: "driver_id");

            migrationBuilder.RenameIndex(
                name: "IX_stints_SessionId",
                schema: "domain",
                table: "stints",
                newName: "ix_stints_session_id");

            migrationBuilder.RenameIndex(
                name: "IX_stints_DriverId",
                schema: "domain",
                table: "stints",
                newName: "ix_stints_driver_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "domain",
                table: "sessions",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "domain",
                table: "sessions",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "domain",
                table: "sessions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Key",
                schema: "domain",
                table: "sessions",
                newName: "key");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "sessions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                schema: "domain",
                table: "sessions",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "MeetingId",
                schema: "domain",
                table: "sessions",
                newName: "meeting_id");

            migrationBuilder.RenameColumn(
                name: "GmtOffset",
                schema: "domain",
                table: "sessions",
                newName: "gmt_offset");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                schema: "domain",
                table: "sessions",
                newName: "end_date");

            migrationBuilder.RenameIndex(
                name: "IX_sessions_MeetingId",
                schema: "domain",
                table: "sessions",
                newName: "ix_sessions_meeting_id");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                schema: "domain",
                table: "session_drivers",
                newName: "team_id");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                schema: "domain",
                table: "session_drivers",
                newName: "driver_id");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "domain",
                table: "session_drivers",
                newName: "session_id");

            migrationBuilder.RenameIndex(
                name: "IX_session_drivers_TeamId",
                schema: "domain",
                table: "session_drivers",
                newName: "ix_session_drivers_team_id");

            migrationBuilder.RenameIndex(
                name: "IX_session_drivers_DriverId",
                schema: "domain",
                table: "session_drivers",
                newName: "ix_session_drivers_driver_id");

            migrationBuilder.RenameColumn(
                name: "Message",
                schema: "domain",
                table: "race_controls",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "Flag",
                schema: "domain",
                table: "race_controls",
                newName: "flag");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "domain",
                table: "race_controls",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Category",
                schema: "domain",
                table: "race_controls",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "race_controls",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "domain",
                table: "race_controls",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "LapNumber",
                schema: "domain",
                table: "race_controls",
                newName: "lap_number");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                schema: "domain",
                table: "race_controls",
                newName: "driver_id");

            migrationBuilder.RenameIndex(
                name: "IX_race_controls_SessionId",
                schema: "domain",
                table: "race_controls",
                newName: "ix_race_controls_session_id");

            migrationBuilder.RenameIndex(
                name: "IX_race_controls_DriverId",
                schema: "domain",
                table: "race_controls",
                newName: "ix_race_controls_driver_id");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "domain",
                table: "pit_stops",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "pit_stops",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "domain",
                table: "pit_stops",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "PitDuration",
                schema: "domain",
                table: "pit_stops",
                newName: "pit_duration");

            migrationBuilder.RenameColumn(
                name: "LapNumber",
                schema: "domain",
                table: "pit_stops",
                newName: "lap_number");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                schema: "domain",
                table: "pit_stops",
                newName: "driver_id");

            migrationBuilder.RenameIndex(
                name: "IX_pit_stops_SessionId",
                schema: "domain",
                table: "pit_stops",
                newName: "ix_pit_stops_session_id");

            migrationBuilder.RenameIndex(
                name: "IX_pit_stops_DriverId",
                schema: "domain",
                table: "pit_stops",
                newName: "ix_pit_stops_driver_id");

            migrationBuilder.RenameColumn(
                name: "Year",
                schema: "domain",
                table: "meetings",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "domain",
                table: "meetings",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Location",
                schema: "domain",
                table: "meetings",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "Key",
                schema: "domain",
                table: "meetings",
                newName: "key");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "meetings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "OfficialName",
                schema: "domain",
                table: "meetings",
                newName: "official_name");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                schema: "domain",
                table: "meetings",
                newName: "date_start");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                schema: "domain",
                table: "meetings",
                newName: "country_name");

            migrationBuilder.RenameColumn(
                name: "CountryKey",
                schema: "domain",
                table: "meetings",
                newName: "country_key");

            migrationBuilder.RenameColumn(
                name: "CircuitShortName",
                schema: "domain",
                table: "meetings",
                newName: "circuit_short_name");

            migrationBuilder.RenameColumn(
                name: "CircuitKey",
                schema: "domain",
                table: "meetings",
                newName: "circuit_key");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "laps",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StSpeed",
                schema: "domain",
                table: "laps",
                newName: "st_speed");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "domain",
                table: "laps",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "SegmentsJson",
                schema: "domain",
                table: "laps",
                newName: "segments_json");

            migrationBuilder.RenameColumn(
                name: "LapNumber",
                schema: "domain",
                table: "laps",
                newName: "lap_number");

            migrationBuilder.RenameColumn(
                name: "LapDurationMs",
                schema: "domain",
                table: "laps",
                newName: "lap_duration_ms");

            migrationBuilder.RenameColumn(
                name: "IsPitOutLap",
                schema: "domain",
                table: "laps",
                newName: "is_pit_out_lap");

            migrationBuilder.RenameColumn(
                name: "I2Speed",
                schema: "domain",
                table: "laps",
                newName: "i2_speed");

            migrationBuilder.RenameColumn(
                name: "I1Speed",
                schema: "domain",
                table: "laps",
                newName: "i1_speed");

            migrationBuilder.RenameColumn(
                name: "FinishLineSpeed",
                schema: "domain",
                table: "laps",
                newName: "finish_line_speed");

            migrationBuilder.RenameColumn(
                name: "DurationSector3Ms",
                schema: "domain",
                table: "laps",
                newName: "duration_sector3_ms");

            migrationBuilder.RenameColumn(
                name: "DurationSector2Ms",
                schema: "domain",
                table: "laps",
                newName: "duration_sector2_ms");

            migrationBuilder.RenameColumn(
                name: "DurationSector1Ms",
                schema: "domain",
                table: "laps",
                newName: "duration_sector1_ms");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                schema: "domain",
                table: "laps",
                newName: "driver_id");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                schema: "domain",
                table: "laps",
                newName: "date_start");

            migrationBuilder.RenameIndex(
                name: "IX_laps_SessionId_DriverId_LapNumber",
                schema: "domain",
                table: "laps",
                newName: "ix_laps_session_id_driver_id_lap_number");

            migrationBuilder.RenameIndex(
                name: "IX_laps_SessionId_DriverId",
                schema: "domain",
                table: "laps",
                newName: "ix_laps_session_id_driver_id");

            migrationBuilder.RenameIndex(
                name: "IX_laps_DriverId",
                schema: "domain",
                table: "laps",
                newName: "ix_laps_driver_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "RecordsCount",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "records_count");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "last_updated");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "error_message");

            migrationBuilder.RenameColumn(
                name: "DataType",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "data_type");

            migrationBuilder.RenameIndex(
                name: "IX_ingestion_statuses_SessionId",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "ix_ingestion_statuses_session_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "domain",
                table: "drivers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NameAcronym",
                schema: "domain",
                table: "drivers",
                newName: "name_acronym");

            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "domain",
                table: "drivers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "HeadshotUrl",
                schema: "domain",
                table: "drivers",
                newName: "headshot_url");

            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "domain",
                table: "drivers",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "domain",
                table: "drivers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "DriverNumber",
                schema: "domain",
                table: "drivers",
                newName: "driver_number");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                schema: "domain",
                table: "drivers",
                newName: "country_code");

            migrationBuilder.RenameColumn(
                name: "BroadcastName",
                schema: "domain",
                table: "drivers",
                newName: "broadcast_name");

            migrationBuilder.RenameIndex(
                name: "IX_drivers_DriverNumber",
                schema: "domain",
                table: "drivers",
                newName: "ix_drivers_driver_number");

            migrationBuilder.AddPrimaryKey(
                name: "pk_teams",
                schema: "domain",
                table: "teams",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_stints",
                schema: "domain",
                table: "stints",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_sessions",
                schema: "domain",
                table: "sessions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_session_drivers",
                schema: "domain",
                table: "session_drivers",
                columns: new[] { "session_id", "driver_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_race_controls",
                schema: "domain",
                table: "race_controls",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pit_stops",
                schema: "domain",
                table: "pit_stops",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_meetings",
                schema: "domain",
                table: "meetings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_laps",
                schema: "domain",
                table: "laps",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ingestion_statuses",
                schema: "domain",
                table: "ingestion_statuses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_drivers",
                schema: "domain",
                table: "drivers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ingestion_statuses_sessions_session_id",
                schema: "domain",
                table: "ingestion_statuses",
                column: "session_id",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_laps_drivers_driver_id",
                schema: "domain",
                table: "laps",
                column: "driver_id",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_laps_sessions_session_id",
                schema: "domain",
                table: "laps",
                column: "session_id",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pit_stops_drivers_driver_id",
                schema: "domain",
                table: "pit_stops",
                column: "driver_id",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pit_stops_sessions_session_id",
                schema: "domain",
                table: "pit_stops",
                column: "session_id",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_race_controls_drivers_driver_id",
                schema: "domain",
                table: "race_controls",
                column: "driver_id",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_race_controls_sessions_session_id",
                schema: "domain",
                table: "race_controls",
                column: "session_id",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_session_drivers_drivers_driver_id",
                schema: "domain",
                table: "session_drivers",
                column: "driver_id",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_session_drivers_sessions_session_id",
                schema: "domain",
                table: "session_drivers",
                column: "session_id",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_session_drivers_teams_team_id",
                schema: "domain",
                table: "session_drivers",
                column: "team_id",
                principalSchema: "domain",
                principalTable: "teams",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sessions_meetings_meeting_id",
                schema: "domain",
                table: "sessions",
                column: "meeting_id",
                principalSchema: "domain",
                principalTable: "meetings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stints_drivers_driver_id",
                schema: "domain",
                table: "stints",
                column: "driver_id",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_stints_sessions_session_id",
                schema: "domain",
                table: "stints",
                column: "session_id",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ingestion_statuses_sessions_session_id",
                schema: "domain",
                table: "ingestion_statuses");

            migrationBuilder.DropForeignKey(
                name: "fk_laps_drivers_driver_id",
                schema: "domain",
                table: "laps");

            migrationBuilder.DropForeignKey(
                name: "fk_laps_sessions_session_id",
                schema: "domain",
                table: "laps");

            migrationBuilder.DropForeignKey(
                name: "fk_pit_stops_drivers_driver_id",
                schema: "domain",
                table: "pit_stops");

            migrationBuilder.DropForeignKey(
                name: "fk_pit_stops_sessions_session_id",
                schema: "domain",
                table: "pit_stops");

            migrationBuilder.DropForeignKey(
                name: "fk_race_controls_drivers_driver_id",
                schema: "domain",
                table: "race_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_race_controls_sessions_session_id",
                schema: "domain",
                table: "race_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_session_drivers_drivers_driver_id",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropForeignKey(
                name: "fk_session_drivers_sessions_session_id",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropForeignKey(
                name: "fk_session_drivers_teams_team_id",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropForeignKey(
                name: "fk_sessions_meetings_meeting_id",
                schema: "domain",
                table: "sessions");

            migrationBuilder.DropForeignKey(
                name: "fk_stints_drivers_driver_id",
                schema: "domain",
                table: "stints");

            migrationBuilder.DropForeignKey(
                name: "fk_stints_sessions_session_id",
                schema: "domain",
                table: "stints");

            migrationBuilder.DropPrimaryKey(
                name: "pk_teams",
                schema: "domain",
                table: "teams");

            migrationBuilder.DropPrimaryKey(
                name: "pk_stints",
                schema: "domain",
                table: "stints");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sessions",
                schema: "domain",
                table: "sessions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_session_drivers",
                schema: "domain",
                table: "session_drivers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_race_controls",
                schema: "domain",
                table: "race_controls");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pit_stops",
                schema: "domain",
                table: "pit_stops");

            migrationBuilder.DropPrimaryKey(
                name: "pk_meetings",
                schema: "domain",
                table: "meetings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_laps",
                schema: "domain",
                table: "laps");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ingestion_statuses",
                schema: "domain",
                table: "ingestion_statuses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_drivers",
                schema: "domain",
                table: "drivers");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "domain",
                table: "teams",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "key",
                schema: "domain",
                table: "teams",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "teams",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "domain",
                table: "teams",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "team_colour",
                schema: "domain",
                table: "teams",
                newName: "TeamColour");

            migrationBuilder.RenameColumn(
                name: "created_at",
                schema: "domain",
                table: "teams",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "compound",
                schema: "domain",
                table: "stints",
                newName: "Compound");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "stints",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "tyre_age_at_start",
                schema: "domain",
                table: "stints",
                newName: "TyreAgeAtStart");

            migrationBuilder.RenameColumn(
                name: "stint_number",
                schema: "domain",
                table: "stints",
                newName: "StintNumber");

            migrationBuilder.RenameColumn(
                name: "session_id",
                schema: "domain",
                table: "stints",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "lap_start",
                schema: "domain",
                table: "stints",
                newName: "LapStart");

            migrationBuilder.RenameColumn(
                name: "lap_end",
                schema: "domain",
                table: "stints",
                newName: "LapEnd");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                schema: "domain",
                table: "stints",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "ix_stints_session_id",
                schema: "domain",
                table: "stints",
                newName: "IX_stints_SessionId");

            migrationBuilder.RenameIndex(
                name: "ix_stints_driver_id",
                schema: "domain",
                table: "stints",
                newName: "IX_stints_DriverId");

            migrationBuilder.RenameColumn(
                name: "type",
                schema: "domain",
                table: "sessions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "domain",
                table: "sessions",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "domain",
                table: "sessions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "key",
                schema: "domain",
                table: "sessions",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "sessions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "start_date",
                schema: "domain",
                table: "sessions",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "meeting_id",
                schema: "domain",
                table: "sessions",
                newName: "MeetingId");

            migrationBuilder.RenameColumn(
                name: "gmt_offset",
                schema: "domain",
                table: "sessions",
                newName: "GmtOffset");

            migrationBuilder.RenameColumn(
                name: "end_date",
                schema: "domain",
                table: "sessions",
                newName: "EndDate");

            migrationBuilder.RenameIndex(
                name: "ix_sessions_meeting_id",
                schema: "domain",
                table: "sessions",
                newName: "IX_sessions_MeetingId");

            migrationBuilder.RenameColumn(
                name: "team_id",
                schema: "domain",
                table: "session_drivers",
                newName: "TeamId");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                schema: "domain",
                table: "session_drivers",
                newName: "DriverId");

            migrationBuilder.RenameColumn(
                name: "session_id",
                schema: "domain",
                table: "session_drivers",
                newName: "SessionId");

            migrationBuilder.RenameIndex(
                name: "ix_session_drivers_team_id",
                schema: "domain",
                table: "session_drivers",
                newName: "IX_session_drivers_TeamId");

            migrationBuilder.RenameIndex(
                name: "ix_session_drivers_driver_id",
                schema: "domain",
                table: "session_drivers",
                newName: "IX_session_drivers_DriverId");

            migrationBuilder.RenameColumn(
                name: "message",
                schema: "domain",
                table: "race_controls",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "flag",
                schema: "domain",
                table: "race_controls",
                newName: "Flag");

            migrationBuilder.RenameColumn(
                name: "date",
                schema: "domain",
                table: "race_controls",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "category",
                schema: "domain",
                table: "race_controls",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "race_controls",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "session_id",
                schema: "domain",
                table: "race_controls",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "lap_number",
                schema: "domain",
                table: "race_controls",
                newName: "LapNumber");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                schema: "domain",
                table: "race_controls",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "ix_race_controls_session_id",
                schema: "domain",
                table: "race_controls",
                newName: "IX_race_controls_SessionId");

            migrationBuilder.RenameIndex(
                name: "ix_race_controls_driver_id",
                schema: "domain",
                table: "race_controls",
                newName: "IX_race_controls_DriverId");

            migrationBuilder.RenameColumn(
                name: "date",
                schema: "domain",
                table: "pit_stops",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "pit_stops",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "session_id",
                schema: "domain",
                table: "pit_stops",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "pit_duration",
                schema: "domain",
                table: "pit_stops",
                newName: "PitDuration");

            migrationBuilder.RenameColumn(
                name: "lap_number",
                schema: "domain",
                table: "pit_stops",
                newName: "LapNumber");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                schema: "domain",
                table: "pit_stops",
                newName: "DriverId");

            migrationBuilder.RenameIndex(
                name: "ix_pit_stops_session_id",
                schema: "domain",
                table: "pit_stops",
                newName: "IX_pit_stops_SessionId");

            migrationBuilder.RenameIndex(
                name: "ix_pit_stops_driver_id",
                schema: "domain",
                table: "pit_stops",
                newName: "IX_pit_stops_DriverId");

            migrationBuilder.RenameColumn(
                name: "year",
                schema: "domain",
                table: "meetings",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "domain",
                table: "meetings",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "location",
                schema: "domain",
                table: "meetings",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "key",
                schema: "domain",
                table: "meetings",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "meetings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "official_name",
                schema: "domain",
                table: "meetings",
                newName: "OfficialName");

            migrationBuilder.RenameColumn(
                name: "date_start",
                schema: "domain",
                table: "meetings",
                newName: "DateStart");

            migrationBuilder.RenameColumn(
                name: "country_name",
                schema: "domain",
                table: "meetings",
                newName: "CountryName");

            migrationBuilder.RenameColumn(
                name: "country_key",
                schema: "domain",
                table: "meetings",
                newName: "CountryKey");

            migrationBuilder.RenameColumn(
                name: "circuit_short_name",
                schema: "domain",
                table: "meetings",
                newName: "CircuitShortName");

            migrationBuilder.RenameColumn(
                name: "circuit_key",
                schema: "domain",
                table: "meetings",
                newName: "CircuitKey");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "laps",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "st_speed",
                schema: "domain",
                table: "laps",
                newName: "StSpeed");

            migrationBuilder.RenameColumn(
                name: "session_id",
                schema: "domain",
                table: "laps",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "segments_json",
                schema: "domain",
                table: "laps",
                newName: "SegmentsJson");

            migrationBuilder.RenameColumn(
                name: "lap_number",
                schema: "domain",
                table: "laps",
                newName: "LapNumber");

            migrationBuilder.RenameColumn(
                name: "lap_duration_ms",
                schema: "domain",
                table: "laps",
                newName: "LapDurationMs");

            migrationBuilder.RenameColumn(
                name: "is_pit_out_lap",
                schema: "domain",
                table: "laps",
                newName: "IsPitOutLap");

            migrationBuilder.RenameColumn(
                name: "i2_speed",
                schema: "domain",
                table: "laps",
                newName: "I2Speed");

            migrationBuilder.RenameColumn(
                name: "i1_speed",
                schema: "domain",
                table: "laps",
                newName: "I1Speed");

            migrationBuilder.RenameColumn(
                name: "finish_line_speed",
                schema: "domain",
                table: "laps",
                newName: "FinishLineSpeed");

            migrationBuilder.RenameColumn(
                name: "duration_sector3_ms",
                schema: "domain",
                table: "laps",
                newName: "DurationSector3Ms");

            migrationBuilder.RenameColumn(
                name: "duration_sector2_ms",
                schema: "domain",
                table: "laps",
                newName: "DurationSector2Ms");

            migrationBuilder.RenameColumn(
                name: "duration_sector1_ms",
                schema: "domain",
                table: "laps",
                newName: "DurationSector1Ms");

            migrationBuilder.RenameColumn(
                name: "driver_id",
                schema: "domain",
                table: "laps",
                newName: "DriverId");

            migrationBuilder.RenameColumn(
                name: "date_start",
                schema: "domain",
                table: "laps",
                newName: "DateStart");

            migrationBuilder.RenameIndex(
                name: "ix_laps_session_id_driver_id_lap_number",
                schema: "domain",
                table: "laps",
                newName: "IX_laps_SessionId_DriverId_LapNumber");

            migrationBuilder.RenameIndex(
                name: "ix_laps_session_id_driver_id",
                schema: "domain",
                table: "laps",
                newName: "IX_laps_SessionId_DriverId");

            migrationBuilder.RenameIndex(
                name: "ix_laps_driver_id",
                schema: "domain",
                table: "laps",
                newName: "IX_laps_DriverId");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "session_id",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "records_count",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "RecordsCount");

            migrationBuilder.RenameColumn(
                name: "last_updated",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "error_message",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "data_type",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "DataType");

            migrationBuilder.RenameIndex(
                name: "ix_ingestion_statuses_session_id",
                schema: "domain",
                table: "ingestion_statuses",
                newName: "IX_ingestion_statuses_SessionId");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "domain",
                table: "drivers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name_acronym",
                schema: "domain",
                table: "drivers",
                newName: "NameAcronym");

            migrationBuilder.RenameColumn(
                name: "last_name",
                schema: "domain",
                table: "drivers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "headshot_url",
                schema: "domain",
                table: "drivers",
                newName: "HeadshotUrl");

            migrationBuilder.RenameColumn(
                name: "full_name",
                schema: "domain",
                table: "drivers",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "domain",
                table: "drivers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "driver_number",
                schema: "domain",
                table: "drivers",
                newName: "DriverNumber");

            migrationBuilder.RenameColumn(
                name: "country_code",
                schema: "domain",
                table: "drivers",
                newName: "CountryCode");

            migrationBuilder.RenameColumn(
                name: "broadcast_name",
                schema: "domain",
                table: "drivers",
                newName: "BroadcastName");

            migrationBuilder.RenameIndex(
                name: "ix_drivers_driver_number",
                schema: "domain",
                table: "drivers",
                newName: "IX_drivers_DriverNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teams",
                schema: "domain",
                table: "teams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stints",
                schema: "domain",
                table: "stints",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sessions",
                schema: "domain",
                table: "sessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_session_drivers",
                schema: "domain",
                table: "session_drivers",
                columns: new[] { "SessionId", "DriverId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_race_controls",
                schema: "domain",
                table: "race_controls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pit_stops",
                schema: "domain",
                table: "pit_stops",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_meetings",
                schema: "domain",
                table: "meetings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_laps",
                schema: "domain",
                table: "laps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ingestion_statuses",
                schema: "domain",
                table: "ingestion_statuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_drivers",
                schema: "domain",
                table: "drivers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ingestion_statuses_sessions_SessionId",
                schema: "domain",
                table: "ingestion_statuses",
                column: "SessionId",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_laps_drivers_DriverId",
                schema: "domain",
                table: "laps",
                column: "DriverId",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_laps_sessions_SessionId",
                schema: "domain",
                table: "laps",
                column: "SessionId",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pit_stops_drivers_DriverId",
                schema: "domain",
                table: "pit_stops",
                column: "DriverId",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pit_stops_sessions_SessionId",
                schema: "domain",
                table: "pit_stops",
                column: "SessionId",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_race_controls_drivers_DriverId",
                schema: "domain",
                table: "race_controls",
                column: "DriverId",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_race_controls_sessions_SessionId",
                schema: "domain",
                table: "race_controls",
                column: "SessionId",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_session_drivers_drivers_DriverId",
                schema: "domain",
                table: "session_drivers",
                column: "DriverId",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_session_drivers_sessions_SessionId",
                schema: "domain",
                table: "session_drivers",
                column: "SessionId",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_session_drivers_teams_TeamId",
                schema: "domain",
                table: "session_drivers",
                column: "TeamId",
                principalSchema: "domain",
                principalTable: "teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sessions_meetings_MeetingId",
                schema: "domain",
                table: "sessions",
                column: "MeetingId",
                principalSchema: "domain",
                principalTable: "meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stints_drivers_DriverId",
                schema: "domain",
                table: "stints",
                column: "DriverId",
                principalSchema: "domain",
                principalTable: "drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stints_sessions_SessionId",
                schema: "domain",
                table: "stints",
                column: "SessionId",
                principalSchema: "domain",
                principalTable: "sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
