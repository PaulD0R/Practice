using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    EmailStatus = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    SmsStatus = table.Column<string>(type: "text", nullable: false),
                    Push = table.Column<string>(type: "text", nullable: true),
                    PushStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
