using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EGTDigital.Migrations
{
    public partial class CurrencyDataIDMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRate_CurrencyDatas_CurrencyDataTimestamp",
                table: "CurrencyRate");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRate_CurrencyDataTimestamp",
                table: "CurrencyRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyDatas",
                table: "CurrencyDatas");

            migrationBuilder.AddColumn<long>(
                name: "CurrencyDataId",
                table: "CurrencyRate",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Timestamp",
                table: "CurrencyDatas",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "CurrencyDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyDatas",
                table: "CurrencyDatas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRate_CurrencyDataId",
                table: "CurrencyRate",
                column: "CurrencyDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRate_CurrencyDatas_CurrencyDataId",
                table: "CurrencyRate",
                column: "CurrencyDataId",
                principalTable: "CurrencyDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyRate_CurrencyDatas_CurrencyDataId",
                table: "CurrencyRate");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyRate_CurrencyDataId",
                table: "CurrencyRate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyDatas",
                table: "CurrencyDatas");

            migrationBuilder.DropColumn(
                name: "CurrencyDataId",
                table: "CurrencyRate");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CurrencyDatas");

            migrationBuilder.AlterColumn<long>(
                name: "Timestamp",
                table: "CurrencyDatas",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyDatas",
                table: "CurrencyDatas",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRate_CurrencyDataTimestamp",
                table: "CurrencyRate",
                column: "CurrencyDataTimestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyRate_CurrencyDatas_CurrencyDataTimestamp",
                table: "CurrencyRate",
                column: "CurrencyDataTimestamp",
                principalTable: "CurrencyDatas",
                principalColumn: "Timestamp",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
