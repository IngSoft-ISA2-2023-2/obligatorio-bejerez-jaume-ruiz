using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaGo.DataAccess.Migrations
{
    public partial class AddProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Pharmacys_PharmacyId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_Presentations_PresentationId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_UnitMeasures_UnitMeasureId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_Drugs_DrugId",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequestDetails_Drugs_DrugId",
                table: "StockRequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drugs",
                table: "Drugs");

            migrationBuilder.RenameTable(
                name: "Drugs",
                newName: "PharmacyItem");

            migrationBuilder.RenameColumn(
                name: "DrugId",
                table: "PurchaseDetails",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_DrugId",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_UnitMeasureId",
                table: "PharmacyItem",
                newName: "IX_PharmacyItem_UnitMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_PresentationId",
                table: "PharmacyItem",
                newName: "IX_PharmacyItem_PresentationId");

            migrationBuilder.RenameIndex(
                name: "IX_Drugs_PharmacyId",
                table: "PharmacyItem",
                newName: "IX_PharmacyItem_PharmacyId");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "PharmacyItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "PharmacyItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Prescription",
                table: "PharmacyItem",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PharmacyItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PharmacyItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Product_PharmacyId",
                table: "PharmacyItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmacyItem",
                table: "PharmacyItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyItem_Product_PharmacyId",
                table: "PharmacyItem",
                column: "Product_PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyItem_Pharmacys_PharmacyId",
                table: "PharmacyItem",
                column: "PharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyItem_Pharmacys_Product_PharmacyId",
                table: "PharmacyItem",
                column: "Product_PharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyItem_Presentations_PresentationId",
                table: "PharmacyItem",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyItem_UnitMeasures_UnitMeasureId",
                table: "PharmacyItem",
                column: "UnitMeasureId",
                principalTable: "UnitMeasures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_PharmacyItem_ItemId",
                table: "PurchaseDetails",
                column: "ItemId",
                principalTable: "PharmacyItem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequestDetails_PharmacyItem_DrugId",
                table: "StockRequestDetails",
                column: "DrugId",
                principalTable: "PharmacyItem",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyItem_Pharmacys_PharmacyId",
                table: "PharmacyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyItem_Pharmacys_Product_PharmacyId",
                table: "PharmacyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyItem_Presentations_PresentationId",
                table: "PharmacyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyItem_UnitMeasures_UnitMeasureId",
                table: "PharmacyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_PharmacyItem_ItemId",
                table: "PurchaseDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockRequestDetails_PharmacyItem_DrugId",
                table: "StockRequestDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmacyItem",
                table: "PharmacyItem");

            migrationBuilder.DropIndex(
                name: "IX_PharmacyItem_Product_PharmacyId",
                table: "PharmacyItem");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PharmacyItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PharmacyItem");

            migrationBuilder.DropColumn(
                name: "Product_PharmacyId",
                table: "PharmacyItem");

            migrationBuilder.RenameTable(
                name: "PharmacyItem",
                newName: "Drugs");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "PurchaseDetails",
                newName: "DrugId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_ItemId",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_DrugId");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyItem_UnitMeasureId",
                table: "Drugs",
                newName: "IX_Drugs_UnitMeasureId");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyItem_PresentationId",
                table: "Drugs",
                newName: "IX_Drugs_PresentationId");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyItem_PharmacyId",
                table: "Drugs",
                newName: "IX_Drugs_PharmacyId");

            migrationBuilder.AlterColumn<int>(
                name: "Stock",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Prescription",
                table: "Drugs",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drugs",
                table: "Drugs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Pharmacys_PharmacyId",
                table: "Drugs",
                column: "PharmacyId",
                principalTable: "Pharmacys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_Presentations_PresentationId",
                table: "Drugs",
                column: "PresentationId",
                principalTable: "Presentations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_UnitMeasures_UnitMeasureId",
                table: "Drugs",
                column: "UnitMeasureId",
                principalTable: "UnitMeasures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_Drugs_DrugId",
                table: "PurchaseDetails",
                column: "DrugId",
                principalTable: "Drugs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockRequestDetails_Drugs_DrugId",
                table: "StockRequestDetails",
                column: "DrugId",
                principalTable: "Drugs",
                principalColumn: "Id");
        }
    }
}
