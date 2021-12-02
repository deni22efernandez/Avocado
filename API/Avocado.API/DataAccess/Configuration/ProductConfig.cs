using Avocado.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.DataAccess.Configuration
{
	//public class ProductConfig : IEntityTypeConfiguration<Product>
	//{
		//public void Configure(EntityTypeBuilder<Product> builder)
		//{
		//	builder.HasKey(x => x.Id);
		//	builder.Property(x => x.Name).IsRequired();
		//	builder.Property(x => x.Description).HasMaxLength(100);
		//	builder.HasOne(x => x.Category).WithMany(x=>x.Products).HasForeignKey(x=>x.CategoryId).IsRequired();
		//}
//	}
}
