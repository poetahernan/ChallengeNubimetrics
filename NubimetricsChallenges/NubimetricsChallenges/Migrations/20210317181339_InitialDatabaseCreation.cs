using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NubimetricsChallenge01Countries.Migrations
{
    public partial class InitialDatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Creamos la tabla user
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    surname = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            //Seeder para user
            migrationBuilder.InsertData(
                  table: "User",
                  columns: new[] { "name", "surname", "email", "password" },
                  values: new object[,]
                  {
                    { "nombre1","apellido1","email1","password1"},
                    { "nombre2","apellido2","email2","password2"},
                    { "nombre3","apellido3","email3","password3"},
                    { "nombre4","apellido4","email4","password4"},
                    { "nombre5","apellido5","email5","password5"},

                  });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
