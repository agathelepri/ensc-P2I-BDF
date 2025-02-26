using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDF.Migrations
{
    /// <inheritdoc />
    public partial class bugeleve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eleves_Eleves_EleveParrainId",
                table: "Eleves");

            migrationBuilder.DropForeignKey(
                name: "FK_Eleves_Familles_FamilleId",
                table: "Eleves");

            migrationBuilder.DropForeignKey(
                name: "FK_Eleves_Promotions_PromotionId",
                table: "Eleves");

            migrationBuilder.DropIndex(
                name: "IX_Eleves_FamilleId",
                table: "Eleves");

            migrationBuilder.DropColumn(
                name: "FamilleId",
                table: "Eleves");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "Eleves",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Prenom",
                table: "Eleves",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Eleves",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Eleves",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "MDP",
                table: "Eleves",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Eleves",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "EleveParrainId",
                table: "Eleves",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "PromotionId1",
                table: "Eleves",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eleves_PromotionId1",
                table: "Eleves",
                column: "PromotionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Eleves_Eleves_EleveParrainId",
                table: "Eleves",
                column: "EleveParrainId",
                principalTable: "Eleves",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eleves_Familles_PromotionId",
                table: "Eleves",
                column: "PromotionId",
                principalTable: "Familles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eleves_Promotions_PromotionId1",
                table: "Eleves",
                column: "PromotionId1",
                principalTable: "Promotions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eleves_Eleves_EleveParrainId",
                table: "Eleves");

            migrationBuilder.DropForeignKey(
                name: "FK_Eleves_Familles_PromotionId",
                table: "Eleves");

            migrationBuilder.DropForeignKey(
                name: "FK_Eleves_Promotions_PromotionId1",
                table: "Eleves");

            migrationBuilder.DropIndex(
                name: "IX_Eleves_PromotionId1",
                table: "Eleves");

            migrationBuilder.DropColumn(
                name: "PromotionId1",
                table: "Eleves");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "Eleves",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Prenom",
                table: "Eleves",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Photo",
                table: "Eleves",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Eleves",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MDP",
                table: "Eleves",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Eleves",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EleveParrainId",
                table: "Eleves",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilleId",
                table: "Eleves",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Eleves_FamilleId",
                table: "Eleves",
                column: "FamilleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eleves_Eleves_EleveParrainId",
                table: "Eleves",
                column: "EleveParrainId",
                principalTable: "Eleves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eleves_Familles_FamilleId",
                table: "Eleves",
                column: "FamilleId",
                principalTable: "Familles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eleves_Promotions_PromotionId",
                table: "Eleves",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
