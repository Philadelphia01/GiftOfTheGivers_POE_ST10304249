using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviationFoundation.Migrations
{
    /// <inheritdoc />
    public partial class FixDonationsForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_AspNetUsers_VolunteerUserId",
                table: "Volunteers");

            migrationBuilder.AlterColumn<string>(
                name: "VolunteerUserId",
                table: "Volunteers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "DonorUserId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations",
                column: "DonorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_AspNetUsers_VolunteerUserId",
                table: "Volunteers",
                column: "VolunteerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_AspNetUsers_VolunteerUserId",
                table: "Volunteers");

            migrationBuilder.AlterColumn<string>(
                name: "VolunteerUserId",
                table: "Volunteers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonorUserId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations",
                column: "DonorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_AspNetUsers_VolunteerUserId",
                table: "Volunteers",
                column: "VolunteerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
