using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressAndAllergensToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventFoodDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventAddress_City",
                table: "Events",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventAddress_PostalCode",
                table: "Events",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventAddress_Region",
                table: "Events",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventAddress_StreetAddress",
                table: "Events",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventFoodDetails",
                table: "Events",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestNotes",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PricePerSeat",
                table: "Events",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventAddress_City",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventAddress_PostalCode",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventAddress_Region",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventAddress_StreetAddress",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventFoodDetails",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GuestNotes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PricePerSeat",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventFoodDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AdditionalFoodItems = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ingredients = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFoodDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventFoodDetails_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventFoodDetails_EventId",
                table: "EventFoodDetails",
                column: "EventId",
                unique: true);
        }
    }
}
