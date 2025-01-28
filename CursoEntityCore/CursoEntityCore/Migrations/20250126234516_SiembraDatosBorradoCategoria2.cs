using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoEntityCore.Migrations
{
    /// <inheritdoc />
    public partial class SiembraDatosBorradoCategoria2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "Categoria_Id",
                keyValue: 182);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Categoria_Id", "Activo", "FechaCreacion", "Nombre" },
                values: new object[] { 182, true, new DateTime(2025, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Categoria 2" });
        }
    }
}
