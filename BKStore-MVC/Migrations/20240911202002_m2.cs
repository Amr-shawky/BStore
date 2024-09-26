using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKStore_MVC.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderBook",
                newName: "TSubPrice");

            migrationBuilder.AddColumn<bool>(
                name: "DelivaryStatus",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Author",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Author_CountryID",
                table: "Author",
                column: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Country_CountryID",
                table: "Author",
                column: "CountryID",
                principalTable: "Country",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Country_CountryID",
                table: "Author");

            migrationBuilder.DropIndex(
                name: "IX_Author_CountryID",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "DelivaryStatus",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Author");

            migrationBuilder.RenameColumn(
                name: "TSubPrice",
                table: "OrderBook",
                newName: "Price");
        }
    }
}
