using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VacationRental.Infra.TransactionalDb.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    Units = table.Column<int>(nullable: false),
                    PreparationTimeInDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Unit = table.Column<int>(nullable: false),
                    RentalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreparationTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    Unit = table.Column<int>(nullable: false),
                    RentalId = table.Column<int>(nullable: false),
                    BookingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreparationTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreparationTimes_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreparationTimes_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "CreateAt", "PreparationTimeInDays", "Units" },
                values: new object[] { 1, new DateTime(2022, 12, 19, 21, 44, 22, 109, DateTimeKind.Utc).AddTicks(3543), 1, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RentalId",
                table: "Bookings",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_PreparationTimes_BookingId",
                table: "PreparationTimes",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PreparationTimes_RentalId",
                table: "PreparationTimes",
                column: "RentalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreparationTimes");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Rentals");
        }
    }
}
