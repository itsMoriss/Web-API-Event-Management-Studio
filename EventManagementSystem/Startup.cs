using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using EventManagementSystem.Data;
using EventManagementSystem.Services;

namespace EventManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure database connection
            services.AddDbContext<EventManagementContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            // Add services
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserService, UserService>();

            // Add Swagger for API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Management API", Version = "v1" });
            });

            // Add other services (authentication, authorization, etc.) if needed

            // Add MVC and enable API controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure production error handling, logging, etc.
                // app.UseExceptionHandler("/Home/Error");
                // app.UseHsts();
            }

            // Enable Swagger UI for API documentation
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Management API V1");
                c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Add authentication and authorization middleware if needed

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
