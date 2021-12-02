using Avocado.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.DataAccess.Configuration
{
	//public class OrderHeaderConfig : IEntityTypeConfiguration<OrderHeader>
	//{
	//	public void Configure(EntityTypeBuilder<OrderHeader> builder)
	//	{
	//		builder.HasKey(x => x.Id);
	//		builder.Property(x => x.OrderDate).IsRequired();
	//		builder.Property(x => x.OrderTotal).IsRequired();
	//		builder.HasOne(x => x.User).WithMany(x => x.OrderHeaders).HasForeignKey(x => x.UserId);
	//		builder.HasOne(x => x.PaymentType).WithMany(x => x.OrderHeaders).HasForeignKey(x => x.PaymentTypeId);
	//	}
	//}
}
