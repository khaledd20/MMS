using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMS.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToMeetings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Users_OrganizerId",
                table: "Meetings");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "RoleName",
                table: "Roles",
                newName: "role_name");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Roles",
                newName: "role_id");

            migrationBuilder.RenameColumn(
                name: "PermissionName",
                table: "Permissions",
                newName: "permission_name");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "Permissions",
                newName: "permission_id");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Meetings",
                newName: "organizer_id");

            migrationBuilder.RenameColumn(
                name: "MeetingId",
                table: "Meetings",
                newName: "meeting_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Meetings",
                newName: "MeetingURL");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_OrganizerId",
                table: "Meetings",
                newName: "IX_Meetings_organizer_id");

            migrationBuilder.AddColumn<string>(
                name: "Agenda",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "Meetings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Attendees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meeting_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendees_Meetings_meeting_id",
                        column: x => x.meeting_id,
                        principalTable: "Meetings",
                        principalColumn: "meeting_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendees_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Minutes",
                columns: table => new
                {
                    minute_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meeting_id = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Minutes", x => x.minute_id);
                    table.ForeignKey(
                        name: "FK_Minutes_Meetings_meeting_id",
                        column: x => x.meeting_id,
                        principalTable: "Meetings",
                        principalColumn: "meeting_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permission_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false),
                    PermissionsPermissionId = table.Column<int>(type: "int", nullable: true),
                    RoleId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Role_Permissions_PermissionsPermissionId",
                        column: x => x.PermissionsPermissionId,
                        principalTable: "Permissions",
                        principalColumn: "permission_id");
                    table.ForeignKey(
                        name: "FK_Permission_Role_Permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "Permissions",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permission_Role_Roles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "Roles",
                        principalColumn: "role_id");
                    table.ForeignKey(
                        name: "FK_Permission_Role_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.status_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_status_id",
                table: "Meetings",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_meeting_id",
                table: "Attendees",
                column: "meeting_id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_user_id",
                table: "Attendees",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Minutes_meeting_id",
                table: "Minutes",
                column: "meeting_id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Role_permission_id",
                table: "Permission_Role",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Role_PermissionsPermissionId",
                table: "Permission_Role",
                column: "PermissionsPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Role_role_id",
                table: "Permission_Role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Role_RoleId1",
                table: "Permission_Role",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Status_status_id",
                table: "Meetings",
                column: "status_id",
                principalTable: "Status",
                principalColumn: "status_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Users_organizer_id",
                table: "Meetings",
                column: "organizer_id",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_role_id",
                table: "Users",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Status_status_id",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Users_organizer_id",
                table: "Meetings");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_role_id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Attendees");

            migrationBuilder.DropTable(
                name: "Minutes");

            migrationBuilder.DropTable(
                name: "Permission_Role");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Users_role_id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_status_id",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Agenda",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "role_name",
                table: "Roles",
                newName: "RoleName");

            migrationBuilder.RenameColumn(
                name: "role_id",
                table: "Roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "permission_name",
                table: "Permissions",
                newName: "PermissionName");

            migrationBuilder.RenameColumn(
                name: "permission_id",
                table: "Permissions",
                newName: "PermissionId");

            migrationBuilder.RenameColumn(
                name: "organizer_id",
                table: "Meetings",
                newName: "OrganizerId");

            migrationBuilder.RenameColumn(
                name: "meeting_id",
                table: "Meetings",
                newName: "MeetingId");

            migrationBuilder.RenameColumn(
                name: "MeetingURL",
                table: "Meetings",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Meetings_organizer_id",
                table: "Meetings",
                newName: "IX_Meetings_OrganizerId");

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsPermissionId = table.Column<int>(type: "int", nullable: false),
                    RolesRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsPermissionId, x.RolesRoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionsPermissionId",
                        column: x => x.PermissionsPermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RolesRoleId",
                        column: x => x.RolesRoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RolesRoleId",
                table: "PermissionRole",
                column: "RolesRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Users_OrganizerId",
                table: "Meetings",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
