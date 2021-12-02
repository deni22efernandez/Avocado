using Avocado.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.DataAccess.Configuration
{
	//public class CategoryConfig : IEntityTypeConfiguration<Category>
	//{
	//	public void Configure(EntityTypeBuilder<Category> builder)
	//	{
	//		builder.HasKey(x => x.Id);
	//		builder.Property(x => x.Name).IsRequired();
	//		builder.HasData(
	//			new Category
	//			{
	//				Id = 1,
	//				Name = "Main course"
	//			},
	//			new Category
	//			{
	//				Id = 2,
	//				Name = "Side dish"
	//			},
	//			new Category
	//			{
	//				Id = 3,
	//				Name = "Drink"
	//			},
	//			new Category
	//			{
	//				Id = 4,
	//				Name = "Dessert"
	//			});
	//	}
	//}

}
