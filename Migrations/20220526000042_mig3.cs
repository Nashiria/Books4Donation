using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksForDonation.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentClearanceDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CreditCardNo",
                table: "Transactions",
                newName: "RequestID");

            migrationBuilder.AddColumn<string>(
                name: "DonatorMail",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DonatorNote",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonatorMail",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DonatorNote",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                table: "Transactions",
                newName: "CreditCardNo");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentClearanceDate",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
