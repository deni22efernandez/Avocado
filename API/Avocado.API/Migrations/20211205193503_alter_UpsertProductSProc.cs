using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.API.Migrations
{
    public partial class alter_UpsertProductSProc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER PROC _sp_ProductUpsert @Id int = NULL, @Name nvarchar(100), @Desc nvarchar(100), " +
                               "@Price decimal, @CategoryId int, @Img nvarchar(100)" +
                               "AS " +
                               "BEGIN " +
                               "IF(@Id IS NULL) " +
                               "BEGIN " +
                               "INSERT INTO dbo.Products (Name, Description, Price, CategoryId, ImgUri) " +
                               "VALUES (@Name, @Desc, @Price, @CategoryId, @Img) " +
                               "return @@Identity " +
                               "END " +
                               "ELSE " +
                               "BEGIN " +
                               "IF(@Img IS NOT NULL) " +
                               "BEGIN " +
                               "UPDATE dbo.Products SET Name=@Name, Description=@Desc, Price=@Price, CategoryId=@CategoryId, ImgUri=@Img " +
                               "WHERE Id=@Id " +
                               "END " +
                               "ELSE " +
                               "BEGIN " +
                               "UPDATE dbo.Products SET Name=@Name, Description=@Desc, Price=@Price, CategoryId=@CategoryId " +
                               "WHERE Id=@Id " +
                               "END " +
                               "END " +
                               "END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROC _sp_ProductUpsert");
        }
    }
}
