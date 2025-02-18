#define LOCAL

using System.Globalization;
using GameCentral.Shared.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace GameCentral {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDbContext<GameCentralContext>(options => {
#if LOCAL
                options.UseSqlite(_configuration["Data:LocalDb"]);
#endif
#if MSSQL
                options.UseSqlServer(_configuration["Data:ConnStr"]);
#endif
            });
            services.AddTransient<IGameService, EfGameCentralRepository>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddDataAnnotationsLocalization().AddViewLocalization();
            services.Configure<RequestLocalizationOptions>(options => {
                var supportedCultures = new[] {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("it"),
                    new CultureInfo("fr"),
                    new CultureInfo("ja"),
                    new CultureInfo("pt"),
                    new CultureInfo("zh"),
                    new CultureInfo("uk"),
                    new CultureInfo("bg"),
                    new CultureInfo("pl"),
                    new CultureInfo("de"),
                };
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var cultures = new[] {
                new CultureInfo("en"),
                new CultureInfo("ru"),
                new CultureInfo("it"),
                new CultureInfo("fr"),
                new CultureInfo("ja"),
                new CultureInfo("pt"),
                new CultureInfo("zh"),
                new CultureInfo("uk"),
                new CultureInfo("bg"),
                new CultureInfo("pl"),
                new CultureInfo("de"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });

            app.UseRouting();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();
        }
    }
}