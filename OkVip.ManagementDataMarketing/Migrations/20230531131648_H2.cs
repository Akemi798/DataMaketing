using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OkVip.ManagementDataMarketing.Migrations
{
    public partial class H2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcess",
                table: "DataMarketing",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80A4CDA6-557D-451D-8337-E2CDB112A834",
                column: "ConcurrencyStamp",
                value: "a724c53e-5bff-4f94-9380-52140af917c4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "A2AED5D0-EFAF-4A7D-950D-DEAC0307583B",
                columns: new[] { "ConcurrencyStamp", "CreateDate", "PasswordHash" },
                values: new object[] { "79ac9954-64b0-4ca1-8d70-3546bbfebada", new DateTime(2023, 5, 31, 20, 16, 47, 856, DateTimeKind.Local).AddTicks(4162), "AQAAAAEAACcQAAAAEAFuradmZb0GQNn4cqLbrtIbqL6eptCJgq/loxMoJJ4LTbmCXwH1gTs9e/gGdAvQkg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcess",
                table: "DataMarketing");

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
    }
}
