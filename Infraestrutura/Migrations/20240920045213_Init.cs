using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GruposTarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    IdUsuario = table.Column<string>(type: "TEXT", nullable: false),
                    Criacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modificacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposTarefas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GrupoTarefasId = table.Column<int>(type: "INTEGER", nullable: false),
                    Conclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    IdUsuario = table.Column<string>(type: "TEXT", nullable: false),
                    Criacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modificacao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_GruposTarefas_GrupoTarefasId",
                        column: x => x.GrupoTarefasId,
                        principalTable: "GruposTarefas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_GrupoTarefasId",
                table: "Tarefas",
                column: "GrupoTarefasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "GruposTarefas");
        }
    }
}
