using System.Text;
using GameCentral.Shared;
using GameCentral.Shared.Database;
using GameCentral.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace GameCentral.API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddMvc();
            services.AddDbContext<GameCentralContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Store")));
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Auth")));
            services.AddTransient<IGameService, EfGameCentralRepository>();
           
            services
                .AddIdentity<ApiUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => 
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
            );

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://dotnetdetail.net",
                    ValidIssuer = "http://dotnetdetail.net",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SECRET_KEY))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();  
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}