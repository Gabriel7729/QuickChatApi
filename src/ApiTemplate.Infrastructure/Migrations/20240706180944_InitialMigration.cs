using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTemplate.Infrastructure.Migrations;

  public partial class InitialMigration : Migration
  {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "Chats",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  IsGroupChat = table.Column<bool>(type: "bit", nullable: false),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Chats", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "UserActions",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  IsEmailValidated = table.Column<bool>(type: "bit", nullable: false),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_UserActions", x => x.Id);
              });

          migrationBuilder.CreateTable(
              name: "Users",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  UserActionId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                  UserActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Users", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Users_UserActions_UserActionId1",
                      column: x => x.UserActionId1,
                      principalTable: "UserActions",
                      principalColumn: "Id");
                  table.ForeignKey(
                      name: "FK_Users_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id");
              });

          migrationBuilder.CreateTable(
              name: "Groups",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Groups", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Groups_Chats_ChatId",
                      column: x => x.ChatId,
                      principalTable: "Chats",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_Groups_Users_AdminId",
                      column: x => x.AdminId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "Messages",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                  SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_Messages", x => x.Id);
                  table.ForeignKey(
                      name: "FK_Messages_Chats_ChatId",
                      column: x => x.ChatId,
                      principalTable: "Chats",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_Messages_Users_SenderId",
                      column: x => x.SenderId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "UserChats",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  ChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_UserChats", x => x.Id);
                  table.ForeignKey(
                      name: "FK_UserChats_Chats_ChatId",
                      column: x => x.ChatId,
                      principalTable: "Chats",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_UserChats_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
              });

          migrationBuilder.CreateTable(
              name: "GroupMembers",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Deleted = table.Column<bool>(type: "bit", nullable: false),
                  DeletedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                  UpdatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                  CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                  UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
              },
              constraints: table =>
              {
                  table.PrimaryKey("PK_GroupMembers", x => x.Id);
                  table.ForeignKey(
                      name: "FK_GroupMembers_Groups_GroupId",
                      column: x => x.GroupId,
                      principalTable: "Groups",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                  table.ForeignKey(
                      name: "FK_GroupMembers_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.NoAction,
                      onUpdate: ReferentialAction.NoAction);
              });

          migrationBuilder.CreateIndex(
              name: "IX_GroupMembers_GroupId",
              table: "GroupMembers",
              column: "GroupId");

          migrationBuilder.CreateIndex(
              name: "IX_GroupMembers_UserId",
              table: "GroupMembers",
              column: "UserId");

          migrationBuilder.CreateIndex(
              name: "IX_Groups_AdminId",
              table: "Groups",
              column: "AdminId");

          migrationBuilder.CreateIndex(
              name: "IX_Groups_ChatId",
              table: "Groups",
              column: "ChatId",
              unique: true);

          migrationBuilder.CreateIndex(
              name: "IX_Messages_ChatId",
              table: "Messages",
              column: "ChatId");

          migrationBuilder.CreateIndex(
              name: "IX_Messages_SenderId",
              table: "Messages",
              column: "SenderId");

          migrationBuilder.CreateIndex(
              name: "IX_UserChats_ChatId",
              table: "UserChats",
              column: "ChatId");

          migrationBuilder.CreateIndex(
              name: "IX_UserChats_UserId",
              table: "UserChats",
              column: "UserId");

          migrationBuilder.CreateIndex(
              name: "IX_Users_UserActionId1",
              table: "Users",
              column: "UserActionId1");

          migrationBuilder.CreateIndex(
              name: "IX_Users_UserId",
              table: "Users",
              column: "UserId");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "GroupMembers");

          migrationBuilder.DropTable(
              name: "Messages");

          migrationBuilder.DropTable(
              name: "UserChats");

          migrationBuilder.DropTable(
              name: "Groups");

          migrationBuilder.DropTable(
              name: "Chats");

          migrationBuilder.DropTable(
              name: "Users");

          migrationBuilder.DropTable(
              name: "UserActions");
      }
  }
