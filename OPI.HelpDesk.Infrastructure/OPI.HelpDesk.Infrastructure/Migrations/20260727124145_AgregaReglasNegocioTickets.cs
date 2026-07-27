using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPI.HelpDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregaReglasNegocioTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ToStatus",
                table: "TicketHistortyLog",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "FromStatus",
                table: "TicketHistortyLog",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ToStatus",
                table: "TicketHistortyLog",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "FromStatus",
                table: "TicketHistortyLog",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
