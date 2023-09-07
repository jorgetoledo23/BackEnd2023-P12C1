using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class primeraMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblDepartamentos",
                columns: table => new
                {
                    Cod_Dpto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblDepartamentos", x => x.Cod_Dpto);
                });

            migrationBuilder.CreateTable(
                name: "TblTrabajadores",
                columns: table => new
                {
                    Rut = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comuna = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cod_Dpto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblTrabajadores", x => x.Rut);
                    table.ForeignKey(
                        name: "FK_TblTrabajadores_TblDepartamentos_Cod_Dpto",
                        column: x => x.Cod_Dpto,
                        principalTable: "TblDepartamentos",
                        principalColumn: "Cod_Dpto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblTrabajadores_Cod_Dpto",
                table: "TblTrabajadores",
                column: "Cod_Dpto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblTrabajadores");

            migrationBuilder.DropTable(
                name: "TblDepartamentos");
        }
    }
}
