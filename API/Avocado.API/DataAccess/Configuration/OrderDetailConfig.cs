using Avocado.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.DataAccess.Configuration
{
	//public class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
	//{
	//	public void Configure(EntityTypeBuilder<OrderDetail> builder)
	//	{
	//		builder.HasKey(x => x.Id);
	//		builder.HasOne(x => x.OrderHeader).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderHeaderId);
	//		builder.HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);
	//	}
	//}
}
