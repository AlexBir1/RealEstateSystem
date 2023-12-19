using DwellingAPI.AppSettings;
using DwellingAPI.Authorization;
using DwellingAPI.DAL.DBContext;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Exceptions;
using DwellingAPI.DAL.UOW;
using DwellingAPI.Filters;
using DwellingAPI.Middlewares;
using DwellingAPI.Middlewares.ApplicationBuilderExtension;
using DwellingAPI.Services.Implementations;
using DwellingAPI.Services.Interfaces;
using DwellingAPI.Services.UOW;
using DwellingAPI.StartupSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Net;
using System.Text;

namespace DwellingAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var fileProviderDirectory = Directory.GetCurrentDirectory();

            builder.Host.UseSerilog((hbc, lc) => lc
            .WriteTo.Console());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("StrConnection"));
            });

            builder.Services.AddIdentityCore<Account>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
            })

            .AddSignInManager<SignInManager<Account>>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()

            .AddEntityFrameworkStores<AppDBContext>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(l =>
            {
                l.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });


            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddScoped<IDBRepository, DBRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<AuthorizationProvider>();
            builder.Services.AddScoped<IApplicationStartup, ApplicationStartup>();
            builder.Services.AddSingleton<AppSettingsProvider>();

            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            app.UseExceptionMiddleware();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(fileProviderDirectory + "\\Photos"),
                RequestPath = new PathString("/Photos")
            });

            app.UseCors(t => t.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //Setup before start
            using (var applicationServices = app.Services.CreateScope())
            {
                var applicationStartupService = applicationServices.ServiceProvider.GetRequiredService<IApplicationStartup>();

                if((await applicationStartupService.CreateDefaultRoles() ) == true)
                    Log.Information("Default roles were added");
                else
                    Log.Warning("Default roles were added");

                try
                {
                    await applicationStartupService.CreateDefaultAdmin();
                }
                catch (Exception ex)
                {
                    Log.Warning("Default admin already added or an error occured");
                }
            }

            app.Run();
        }
    }
}