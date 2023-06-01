using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OkVip.ManagementDataMarketing.Migrations
{
    public partial class H1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataMarketing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BuyDate = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DataBuyOfWeb = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDuplicate = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataMarketing", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80A4CDA6-557D-451D-8337-E2CDB112A834",
                column: "ConcurrencyStamp",
                value: "5a7e6f81-96a2-4d89-af43-ede09a3d3871");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A2AED5D0-EFAF-4A7D-950D-DEAC0307583B",
                columns: new[] { "ConcurrencyStamp", "CreateDate", "PasswordHash" },
                values: new object[] { "d07102bd-b90e-4ad1-a1fd-8b8d624fee49", new DateTime(2023, 4, 27, 19, 55, 24, 988, DateTimeKind.Local).AddTicks(7231), "AQAAAAEAACcQAAAAEBxQphaKWJiidpKKpBLVd0uipyLHx+U3tSjJGWW4aO1KZQVG2rHQoElBm1cMIkgy3g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataMarketing");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80A4CDA6-557D-451D-8337-E2CDB112A834",
                column: "ConcurrencyStamp",
                value: "65ec44e9-4620-4c7d-ae08-f9b41eb13155");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A2AED5D0-EFAF-4A7D-950D-DEAC0307583B",
                columns: new[] { "ConcurrencyStamp", "CreateDate", "PasswordHash" },
                values: new object[] { "79833499-e5a8-441e-ab25-fe3048b0b567", new DateTime(2023, 4, 27, 15, 36, 49, 473, DateTimeKind.Local).AddTicks(2049), "AQAAAAEAACcQAAAAEHDzXgBxAm9GrU/4bYR+5VbiKcmyN1lstWOvO6qhkTOlIceL9tXwOs5+Ll1tyyOrUg==" });
        }
    }
}
