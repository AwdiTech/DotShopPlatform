using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DotShopPlatform.DAL;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DotShopPlatform
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		public void ConfigureServices(IServiceCollection services)
		{
			// CORS setup for local development
			services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins, builder =>
				{
					builder.WithOrigins("http://localhost:3000") // Adjust if your front-end runs on a different port
						   .AllowAnyHeader()
						   .AllowAnyMethod();
				});
			});

			services.AddControllers(); // MVC pattern support

			// Database context setup
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			// JWT Authentication Setup
			var appSettings = Configuration.GetSection("AppSettings").GetValue<string>("Secret");
			var key = Encoding.ASCII.GetBytes(appSettings);
			services.AddAuthentication(scheme =>
			{
				scheme.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseCors(MyAllowSpecificOrigins);
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
