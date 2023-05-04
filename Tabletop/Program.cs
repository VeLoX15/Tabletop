using System.Reflection;
using Tabletop.Core.Interfaces;
using Tabletop.Core.Services;

namespace Tabletop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor()
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                });

            builder.Services.AddScoped<IDbProviderService, MySqlProviderService>();
            builder.Services.AddScoped<UnitService>();
            builder.Services.AddScoped<WeaponService>();
            builder.Configuration.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), false, true);

            // FluentValidation
            //builder.Services.AddValidatorsFromAssembly(Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FormPortal.Core.Validators.dll")));
            var app = builder.Build();
            using var serviceScope = app.Services.CreateScope();

            var services = serviceScope.ServiceProvider;
            var dbProviderService = services.GetRequiredService<IDbProviderService>()!;

            await AppdatenService.Init(builder.Configuration, dbProviderService);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

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