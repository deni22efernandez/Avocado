using Microsoft.EntityFrameworkCore.Migrations;

namespace Avocado.API.Migrations
{
    public partial class SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create proc _sp_UpdateProduct @ProdId int, @Name nvarchar(50), @Desc nvarchar(100), @Price decimal, @CategoryId int, @Img nvarchar(100) " +                 
                                "AS " + "SET NOCOUNT ON " + "BEGIN " + "BEGIN TRY "  +
                                "IF(@Img!=null)"+
                                "BEGIN "+
                                "Update dbo.Products " +
                                "SET Name=@Name, Description=@Desc, Price=@Price, CategoryId=@CategoryId, ImgUri=@Img WHERE Id=@ProdId "+
                                "END "+
                                "ELSE "+
                                "BEGIN "+
                                "Update dbo.Products " +
                                "SET Name=@Name, Description=@Desc, Price=@Price, CategoryId=@CategoryId WHERE Id=@ProdId " +
                                "END " +
                                "END TRY " +
                                "BEGIN CATCH " +
                                "END CATCH " +
                                "END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("@DROP PROCEDURE _sp_UpdateProduct");
        }
    }
}
