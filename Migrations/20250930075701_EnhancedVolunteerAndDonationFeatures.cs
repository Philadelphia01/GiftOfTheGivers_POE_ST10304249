using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviationFoundation.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedVolunteerAndDonationFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedVolunteerId",
                table: "VolunteerTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "VolunteerTasks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VolunteerTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "VolunteerTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentVolunteerCount",
                table: "VolunteerTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "VolunteerTasks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxVolunteers",
                table: "VolunteerTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "VolunteerTasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequiredSkills",
                table: "VolunteerTasks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDistributed",
                table: "Donations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistributedByUserId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistributionNotes",
                table: "Donations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Donations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Donations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "VolunteerCommunications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipientUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelatedTaskId = table.Column<int>(type: "int", nullable: true),
                    MessageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerCommunications", x => x.Id);
                    // Foreign key constraints removed to avoid cascade path issues
                    // table.ForeignKey(
                    //     name: "FK_VolunteerCommunications_AspNetUsers_RecipientUserId",
                    //     column: x => x.RecipientUserId,
                    //     principalTable: "AspNetUsers",
                    //     principalColumn: "Id");
                    // table.ForeignKey(
                    //     name: "FK_VolunteerCommunications_AspNetUsers_SenderUserId",
                    //     column: x => x.SenderUserId,
                    //     principalTable: "AspNetUsers",
                    //     principalColumn: "Id",
                    //     onDelete: ReferentialAction.Cascade);
                    // table.ForeignKey(
                    //     name: "FK_VolunteerCommunications_VolunteerTasks_RelatedTaskId",
                    //     column: x => x.RelatedTaskId,
                    //     principalTable: "VolunteerTasks",
                    //     principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTasks_AssignedVolunteerId",
                table: "VolunteerTasks",
                column: "AssignedVolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerTasks_CreatedByUserId",
                table: "VolunteerTasks",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DistributedByUserId",
                table: "Donations",
                column: "DistributedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerCommunications_RecipientUserId",
                table: "VolunteerCommunications",
                column: "RecipientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerCommunications_RelatedTaskId",
                table: "VolunteerCommunications",
                column: "RelatedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerCommunications_SenderUserId",
                table: "VolunteerCommunications",
                column: "SenderUserId");

            // Foreign key constraints removed to avoid cascade path issues
            // migrationBuilder.AddForeignKey(
            //     name: "FK_Donations_AspNetUsers_DistributedByUserId",
            //     table: "Donations",
            //     column: "DistributedByUserId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Donations_AspNetUsers_DonorUserId",
            //     table: "Donations",
            //     column: "DonorUserId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_VolunteerTasks_AspNetUsers_AssignedVolunteerId",
            //     table: "VolunteerTasks",
            //     column: "AssignedVolunteerId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_VolunteerTasks_AspNetUsers_CreatedByUserId",
            //     table: "VolunteerTasks",
            //     column: "CreatedByUserId",
            //     principalTable: "AspNetUsers",
            //     principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_DistributedByUserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerTasks_AspNetUsers_AssignedVolunteerId",
                table: "VolunteerTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerTasks_AspNetUsers_CreatedByUserId",
                table: "VolunteerTasks");

            migrationBuilder.DropTable(
                name: "VolunteerCommunications");

            migrationBuilder.DropIndex(
                name: "IX_VolunteerTasks_AssignedVolunteerId",
                table: "VolunteerTasks");

            migrationBuilder.DropIndex(
                name: "IX_VolunteerTasks_CreatedByUserId",
                table: "VolunteerTasks");

            migrationBuilder.DropIndex(
                name: "IX_Donations_DistributedByUserId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "CurrentVolunteerCount",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "MaxVolunteers",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "RequiredSkills",
                table: "VolunteerTasks");

            migrationBuilder.DropColumn(
                name: "DateDistributed",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DistributedByUserId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DistributionNotes",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Donations");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedVolunteerId",
                table: "VolunteerTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_DonorUserId",
                table: "Donations",
                column: "DonorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
