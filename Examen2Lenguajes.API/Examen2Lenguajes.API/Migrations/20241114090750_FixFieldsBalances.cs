using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen2Lenguajes.API.Migrations
{
    /// <inheritdoc />
    public partial class FixFieldsBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountEntityAccountNumber",
                schema: "dbo",
                table: "balances",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "journal_entry_id",
                schema: "dbo",
                table: "balances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_balances_AccountEntityAccountNumber",
                schema: "dbo",
                table: "balances",
                column: "AccountEntityAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_balances_journal_entry_id",
                schema: "dbo",
                table: "balances",
                column: "journal_entry_id");

            migrationBuilder.AddForeignKey(
                name: "FK_balances_accounts_AccountEntityAccountNumber",
                schema: "dbo",
                table: "balances",
                column: "AccountEntityAccountNumber",
                principalSchema: "dbo",
                principalTable: "accounts",
                principalColumn: "account_number");

            migrationBuilder.AddForeignKey(
                name: "FK_balances_journal_entries_journal_entry_id",
                schema: "dbo",
                table: "balances",
                column: "journal_entry_id",
                principalSchema: "dbo",
                principalTable: "journal_entries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_balances_accounts_AccountEntityAccountNumber",
                schema: "dbo",
                table: "balances");

            migrationBuilder.DropForeignKey(
                name: "FK_balances_journal_entries_journal_entry_id",
                schema: "dbo",
                table: "balances");

            migrationBuilder.DropIndex(
                name: "IX_balances_AccountEntityAccountNumber",
                schema: "dbo",
                table: "balances");

            migrationBuilder.DropIndex(
                name: "IX_balances_journal_entry_id",
                schema: "dbo",
                table: "balances");

            migrationBuilder.DropColumn(
                name: "AccountEntityAccountNumber",
                schema: "dbo",
                table: "balances");

            migrationBuilder.DropColumn(
                name: "journal_entry_id",
                schema: "dbo",
                table: "balances");
        }
    }
}
