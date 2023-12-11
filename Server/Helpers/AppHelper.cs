using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace Server.Helpers
{
	public class Application
	{
        public WebApplication WebApp { get; set; }
        public Application(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			#region Build Configurations

			// Build-in logger disabling:
			builder.Logging.ClearProviders();
			
			// Custom serialization for NativeAOT publication:
			//builder.Services.ConfigureHttpJsonOptions(options =>
			//{
			//	options.SerializerOptions.TypeInfoResolver = new Serializer();
			//});

			// Auth:
			builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
			builder.Services.AddAuthorizationBuilder();

			// Swagger:
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Controllers:
			builder.Services.AddControllers();

			// Code-first Entity FW DbContext with Auth:
			builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("DataSource=YouAreLite.db"));

			builder.Services.AddIdentityCore<Account>()
				.AddEntityFrameworkStores<DataContext>()
				.AddApiEndpoints();

			#endregion

			this.WebApp = builder.Build();
        }

        public void Run()
        {
			#region Application & Middlware Configurations

			// Configure the HTTP request pipeline.
			if (this.WebApp.Environment.IsDevelopment())
			{
				this.WebApp.UseSwagger();
				this.WebApp.UseSwaggerUI();

				// Creating Database if not yet created:
				var scope = this.WebApp.Services.CreateScope();
				var db = scope.ServiceProvider.GetRequiredService<DataContext>();
				db.Database.EnsureCreated();
			}

			this.WebApp.MapControllers();
			this.WebApp.MapIdentityApi<Account>();
			
			this.WebApp.UseHttpsRedirection();

			#endregion

			Title.header();

			this.WebApp.Run();
        }
    }
}
