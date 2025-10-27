using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spaV1.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToRendezVous : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RendezVous",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "RendezVous");
        }
    }
}
