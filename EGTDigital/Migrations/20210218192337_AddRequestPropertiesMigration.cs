using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EGTDigital.Migrations
{
    public partial class AddRequestPropertiesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRate_CurrencyDatas_CurrencyDataId",
                table: "CurrencyRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRate",
                table: "CurrencyRate");

            migrationBuilder.RenameTable(
                name: "CurrencyRate",
                newName: "CurrencyRates");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRate_CurrencyDataId",
                table: "CurrencyRates",
                newName: "IX_CurrencyRates_CurrencyDataId");

            migrationBuilder.AlterColumn<string>(
                name: "RequestId",
                table: "Requests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Requests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Timestamp",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRates",
                table: "CurrencyRates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRates_CurrencyDatas_CurrencyDataId",
                table: "CurrencyRates",
                column: "CurrencyDataId",
                principalTable: "CurrencyDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRates_CurrencyDatas_CurrencyDataId",
                table: "CurrencyRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyRates",
                table: "CurrencyRates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Requests");

            migrationBuilder.RenameTable(
                name: "CurrencyRates",
                newName: "CurrencyRate");

            migrationBuilder.RenameIndex(
                name: "IX_CurrencyRates_CurrencyDataId",
                table: "CurrencyRate",
                newName: "IX_CurrencyRate_CurrencyDataId");

            migrationBuilder.AlterColumn<string>(
                name: "RequestId",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "RequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyRate",
                table: "CurrencyRate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRate_CurrencyDatas_CurrencyDataId",
                table: "CurrencyRate",
                column: "CurrencyDataId",
                principalTable: "CurrencyDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
