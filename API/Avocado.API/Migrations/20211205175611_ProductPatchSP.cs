using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.API.Migrations
{
    public partial class ProductPatchSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE PROC _sp_ProductUpsert @Id int, @Name nvarchar(100), @Desc nvarchar(100), " +
                                 "@Price decimal, @CategoryId int, @Img nvarchar(100) " +
                                 "AS " +
                                 "BEGIN " +
                                 "IF(@Id = 0)" +
                                 "BEGIN " +
                                 "INSERT INTO dbo.Products (Name, Description, Price, CategoryId, ImgUri) " +
                                 "VALUES (@Name, @Desc, @Price, @CategoryId, @Img) " +
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
            migrationBuilder.Sql("DROP PROCEDURE _sp_ProductPatchSP");
        }
    }
}
