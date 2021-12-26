using Avocado.WEB.Common;
using Avocado.WEB.Repository;
using Avocado.WEB.Repository.IRepository;
using Avocado.WEB.SMTP;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
			services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(x =>
				{					
					x.LoginPath = "/Account/Login";
					x.AccessDeniedPath = "/Account/AccessDenied";
					x.Cookie.HttpOnly = true;
					x.Cookie.IsEssential = true;
					x.ExpireTimeSpan = TimeSpan.FromMinutes(10);
					x.SlidingExpiration = true;
				});
			services.AddDistributedMemoryCache();
			services.AddSession(x=> {
				x.Cookie.IsEssential = true;
				x.IdleTimeout = TimeSpan.FromMinutes(5);
				x.Cookie.HttpOnly = true;
				
			});
			services.AddHttpContextAccessor();
			services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
			services.Configure<EmailSenderConfig>(Configuration.GetSection("Smtp"));
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddRazorPages();
			services.AddControllersWithViews().AddRazorRuntimeCompilation();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			StripeConfiguration.ApiKey=Configuration.GetSection("Stripe:SecretKey").Get<string>();
			app.UseSession();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
