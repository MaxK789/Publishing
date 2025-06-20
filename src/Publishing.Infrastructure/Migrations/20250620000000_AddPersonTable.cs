#nullable disable
using Microsoft.EntityFrameworkCore.Migrations;

namespace Publishing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonTable : Migration
    {
        /// <inheritdoc />
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}

