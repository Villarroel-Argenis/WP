#pragma warning disable // Archivo generado automáticamente por Entity Framework Core - se suprime todas las advertencias
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WP.Infrastructure.Persistence.Data.Migrations
{
    /// <summary>
    /// Migración inicial para crear la tabla de cuentas.
    /// </summary>
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <summary>
        /// Aplica la migración creando la tabla de cuentas.
        /// </summary>
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    balance_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    balance_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });
        }

        /// <summary>
        /// Revierte la migración eliminando la tabla de cuentas.
        /// </summary>
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}

#pragma warning restore
