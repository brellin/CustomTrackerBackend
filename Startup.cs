using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CustomTrackerBackend.Models;
using Microsoft.OpenApi.Models;


namespace CustomTrackerBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<UserContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Custom Tracker Backend",
                Version = "v1",
                Description = "Backend for Custom Tracker",
                License = new OpenApiLicense
                {
                    Name = "Using MIT Open Source License",
                    Url = new Uri("https://opensource.org/licenses/MIT"),
                }
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Custom Tracker Backend");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
