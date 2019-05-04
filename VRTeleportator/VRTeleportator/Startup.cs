using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using VRTeleportator.Models;
using VRTeleportator.Services.Interfaces;

namespace VRTeleportator
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
            services.AddSingleton<ISecretKey, SecretKeyService>();
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
#if DEBUG
            services.AddDbContext<AppDataBase>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalDB"),
                b => b.MigrationsAssembly("VRTeleportator")));
#else
            services.AddDbContext<AppDataBase>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("VRTeleportator"),
                b => b.MigrationsAssembly("VRTeleportator")));
#endif
            services.AddIdentity<User, IdentityRole<Guid>>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<AppDataBase>()
                .AddDefaultTokenProviders();

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JwtOptions")["Key"]));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidIssuer = Configuration.GetSection("JwtOptions")["Issuer"],

                      ValidateAudience = true,
                      ValidAudience = Configuration.GetSection("JwtOptions")["Audience"],

                      ValidateLifetime = true,

                      IssuerSigningKey = key,

                      ValidateIssuerSigningKey = true
                  };
              });
            //services.Configure<FormOptions>(x =>
            //{
            //    x.ValueLengthLimit = int.MaxValue;
            //    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            //});
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                
            });
            app.UseCors(config =>
                config.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials());
            app.UseMvc();
        }
    }
}
