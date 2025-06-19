using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Publishing.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    idPerson = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(nullable: false),
                    LName = table.Column<string>(nullable: false),
                    emailPerson = table.Column<string>(nullable: false),
                    typePerson = table.Column<string>(nullable: false),
                    phonePerson = table.Column<string>(nullable: true),
                    faxPerson = table.Column<string>(nullable: true),
                    addressPerson = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.idPerson);
                });

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
                    price = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
