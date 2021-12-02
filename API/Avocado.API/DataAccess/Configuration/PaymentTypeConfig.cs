using Avocado.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.DataAccess.Configuration
{
	public class PaymentTypeConfig : IEntityTypeConfiguration<PaymentType>
	{
		public void Configure(EntityTypeBuilder<PaymentType> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Description).HasMaxLength(50);
			builder.HasData(new PaymentType
			{
				Id = 1,
				Description = "Credit card"
			},
			new PaymentType
			{
				Id = 2,
				Description = "Cash"
			});
		}
	}
}
