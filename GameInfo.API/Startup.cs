using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameInfo.Core.Interfaces;
using GameInfo.Core.Model;
using GameInfo.Infrastructure.AuthEvents;
using GameInfo.Infrastructure.Repository;
using GameInfo.Infrastructure.Repository.Context;
using GameInfo.Infrastructure.Repository.Interfaces;
using GameInfo.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameInfo.API
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

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins(Configuration["AppSettings:ClientAppUrl"])
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();

            services.AddDbContext<GameInfoContext>(options => options.UseSqlServer(
                                                   Configuration.GetConnectionString("Connection"))
                                                   );

            services.AddTransient<IRepository, GameInfoRepository>();
            services.AddTransient<IAuthenticateService, AuthenticateService>();
            services.AddTransient<ITokenBuilderService, TokenBuilderService>();
            services.AddTransient<ITokenProviderService, TokenProviderService>();
            services.AddTransient<ILoggerService, LoggerService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<DevDatabaseSetup>();
            services.AddTransient<JWTAuthenticationEvents>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Game Info API", Version = "v1" });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }});
            });

            services.AddOptions<AppSettings>();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //Get token settings key
            var settings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.Default.GetBytes(settings.TokenSettings.Key);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DevDatabaseSetup seedDatabase)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seedDatabase.Setup();
            }
            
            //app.Use(async (context, next) =>
            //{
            //    context.Request.EnableBuffering();
            //    await next();
            //});

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.DocumentTitle = "GameInfo API";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.DisplayRequestDuration();
            });

            app.UseRouting();
          

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<Handlers.LogMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
