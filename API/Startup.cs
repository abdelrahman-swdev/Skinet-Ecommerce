using System.IO;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Infrastructure.identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<StoreContext>(options => {
                options.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<AppIdentityDbContext>(op => {
                op.UseNpgsql(_config.GetConnectionString("IdentityConnection"));
            });

            // add application services
            services.AddApplicationServices();

            // add identity services
            services.AddIdentityServices(_config);
            
            // add swagger
            services.AddSwaggerDocumentation();

            // add automapper
            services.AddAutoMapper(typeof(MappingProfiles));

            // add Redis
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            // add cors
            services.AddCors(op => {
                op.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // exception middleware
            app.UseMiddleware<ExceptionMiddleware>();

            
            // add middleware for redirect to errors path when requesting endpoint that doesnot exist
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            // use static files
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")
                ),
                RequestPath = "/content"
            });

            // use cors
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            // user swagger middleware
            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
