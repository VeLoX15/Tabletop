using Dapper;
using DbController.TypeHandler;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using Tabletop.Core.Services;

namespace Tabletop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            SqlMapper.AddTypeHandler(typeof(Guid), new GuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            var config = builder.Configuration;

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor()
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, configureOptions =>
                {
                });

            builder.Services.AddScoped<PermissionService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UnitService>();
            builder.Services.AddScoped<AbilityService>();
            builder.Services.AddScoped<WeaponService>();
            builder.Services.AddScoped<GamemodeService>();
            builder.Services.AddScoped<FractionService>();
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<PlayerService>();
            builder.Services.AddScoped<TemplateService>();
            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Languages";
            });

            builder.Configuration.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), false, true);

            builder.Services.AddControllersWithViews();

            // FluentValidation
            builder.Services.AddValidatorsFromAssembly(Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tabletop.Core.dll")));
            var app = builder.Build();
            using var serviceScope = app.Services.CreateScope();

            var services = serviceScope.ServiceProvider;


            await AppdataService.InitAsync(builder.Configuration);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(AppdataService.SupportedCultures[0].Name)
                .AddSupportedCultures(AppdataService.SupportedCultureCodes)
                .AddSupportedUICultures(AppdataService.SupportedCultureCodes);

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.MapControllers();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }

        public static string GetVersion()
        {
            return Assembly.GetEntryAssembly()!.GetName()!.Version!.ToString(3);
        }
    }
}