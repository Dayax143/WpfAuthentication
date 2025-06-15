using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfEfAuthen.Migrations
{
    /// <inheritdoc />
    public partial class hash2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "tblUser",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "tblUser",
                newName: "Password");
        }
    }
}
