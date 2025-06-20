#nullable disable
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Publishing.Infrastructure.Migrations
{
    public partial class AddProductPassOrdersTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pass",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    password = table.Column<string>(nullable: false),
                    idPerson = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pass", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pass_Person_idPerson",
                        column: x => x.idPerson,
                        principalTable: "Person",
                        principalColumn: "idPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    idProduct = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idPerson = table.Column<int>(nullable: false),
                    typeProduct = table.Column<string>(nullable: false),
                    nameProduct = table.Column<string>(nullable: false),
                    pagesNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.idProduct);
                    table.ForeignKey(
                        name: "FK_Product_Person_idPerson",
                        column: x => x.idPerson,
                        principalTable: "Person",
                        principalColumn: "idPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    idOrder = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idProduct = table.Column<int>(nullable: true),
                    idPerson = table.Column<int>(nullable: false),
                    namePrintery = table.Column<string>(nullable: false),
                    dateOrder = table.Column<DateTime>(nullable: false),
                    dateStart = table.Column<DateTime>(nullable: false),
                    dateFinish = table.Column<DateTime>(nullable: false),
                    statusOrder = table.Column<string>(nullable: false),
                    tirage = table.Column<int>(nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.idOrder);
                    table.ForeignKey(
                        name: "FK_Orders_Person_idPerson",
                        column: x => x.idPerson,
                        principalTable: "Person",
                        principalColumn: "idPerson",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pass_idPerson",
                table: "Pass",
                column: "idPerson",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_idPerson",
                table: "Product",
                column: "idPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_idPerson",
                table: "Orders",
                column: "idPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_idProduct",
                table: "Orders",
                column: "idProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Pass");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
