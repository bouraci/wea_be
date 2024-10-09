using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFModels.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            // Populate the books table with initial data
            migrationBuilder.InsertData(
                table: "books",
                columns: new[] { "Id", "Title", "Authors", "Publisher", "PublishedDate", "ISBN" },
                values: new object[,]
                {
                    { 1, "The Great Gatsby", "F. Scott Fitzgerald", "Scribner", new DateTime(1925, 4, 10), 12345 },
                    { 2, "1984", "George Orwell", "Secker & Warburg", new DateTime(1949, 6, 8), 98765 },
                    { 3, "To Kill a Mockingbird", "Harper Lee", "J.B. Lippincott & Co.", new DateTime(1960, 7, 11), 1232233 },
                    { 4, "Pride and Prejudice", "Jane Austen", "T. Egerton", new DateTime(1813, 1, 28), 32132 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "books");
        }
    }
}
