using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace task.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "offices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    city_code = table.Column<int>(type: "integer", nullable: false),
                    uuid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    country_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    address_region = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address_city = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address_street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address_house_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address_apartment = table.Column<int>(type: "integer", nullable: true),
                    work_time = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "phones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    office_id = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    additional = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones", x => x.id);
                    table.ForeignKey(
                        name: "FK_phones_offices_office_id",
                        column: x => x.office_id,
                        principalTable: "offices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_offices_city_code",
                table: "offices",
                column: "city_code");

            migrationBuilder.CreateIndex(
                name: "ix_offices_code",
                table: "offices",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_offices_uuid",
                table: "offices",
                column: "uuid");

            migrationBuilder.CreateIndex(
                name: "ix_phones_office_id",
                table: "phones",
                column: "office_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "phones");

            migrationBuilder.DropTable(
                name: "offices");
        }
    }
}