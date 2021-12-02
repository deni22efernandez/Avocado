using Avocado.API.DataAccess.Configuration;
using Avocado.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.API.DataAccess
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
		{

		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<PaymentType> PaymentTypes { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			//builder.ApplyConfiguration(new CategoryConfig());
			//builder.ApplyConfiguration(new OrderDetailConfig());
			//builder.ApplyConfiguration(new OrderHeaderConfig());
			//builder.ApplyConfiguration(new PaymentTypeConfig());
			//builder.ApplyConfiguration(new ProductConfig());
			//builder.ApplyConfiguration(new UserConfig());
		}
	}
}
