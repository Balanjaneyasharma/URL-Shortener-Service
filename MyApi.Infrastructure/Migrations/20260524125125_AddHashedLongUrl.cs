using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHashedLongUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add  column  allowing nulls
            migrationBuilder.AddColumn<string>(
                name: "HashedLongUrl",
                table: "ShortUrlTable",
                type: "text",
                nullable: true
            );

            // Activate the Postgres crypto extension if it isn't already active
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pgcrypto;");

            // Backfill existing records using native SHA-256 matching your C# generator
            // digest() outputs bytes, encode() converts it to a Hex string like Convert.ToHexString()
            migrationBuilder.Sql("UPDATE \"ShortUrlTable\" SET \"HashedLongUrl\" = encode(digest(\"LongUrl\", 'sha256'), 'hex') WHERE \"HashedLongUrl\" IS NULL;");

            // removing nullability constraint to enforce future records must have hashed long url
            migrationBuilder.AlterColumn<string>(
                name: "HashedLongUrl",
                table: "ShortUrlTable",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedLongUrl",
                table: "ShortUrlTable");
        }
    }
}
