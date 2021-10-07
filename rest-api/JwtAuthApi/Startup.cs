using System;
using System.Text;
using JwtAuthApi.Adapters;
using JwtAuthApi.Commands;
using JwtAuthApi.Ports;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			
			var jwtSecretKey = Configuration.GetValue<string>("JwtSettings:Secret");
			services.Configure<AuthTokenOptions>(Configuration.GetSection(AuthTokenOptions.SectionName));
			services.AddSingleton<IInMemoryDatabase, InMemoryDatabase>();
			services.AddScoped<IProcessingInProgress, ProcessingInProgress>();
			services.AddSingleton<ICommandFactory, CommandFactory>();
			services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();

			//Auth config start
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});
			//Auth config end


			// allow connection from ionic app that is running in browser
			services.AddCors(options =>
			{
				options.AddPolicy("AngularApp",
					builder =>
						builder
							.WithOrigins("http://localhost:4200")
							.AllowAnyHeader()
							.AllowAnyMethod()
							.AllowCredentials());
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors("AngularApp");

			app.UseRouting();
			//Auth setup start
			app.UseAuthentication();
			app.UseAuthorization();
			//Auth setup end
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
