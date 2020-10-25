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
            services.AddDbContext<GameCentralContext>(options => options.UseSqlServer(_configuration["Data:ConnStr"]));
            services.AddTransient<IGameService, EfGameCentralRepository>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddViewLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var cultures = new[] {
                new CultureInfo("en"),
                new CultureInfo("ru"),
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