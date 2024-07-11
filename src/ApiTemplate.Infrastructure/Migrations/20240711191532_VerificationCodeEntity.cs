using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTemplate.Infrastructure.Migrations;

  public partial class VerificationCodeEntity : Migration
  {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.CreateTable(
              name: "VerificationCodes",
              columns: table => new
              {
                  Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                  Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  SentTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                  ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                  Attemps = table.Column<int>(type: "int", nullable: false),
                  Status = table.Column<int>(type: "int", nullable: false),
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
                  table.PrimaryKey("PK_VerificationCodes", x => x.Id);
              });
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
          migrationBuilder.DropTable(
              name: "VerificationCodes");
      }
  }
