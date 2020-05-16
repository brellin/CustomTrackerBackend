using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CustomTrackerBackend.Models;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace CustomTrackerBackend
{
    public class Startup
    {
        private string pgConnection;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            env = environment;
        }

        internal static IConfiguration Configuration { get; private set; }
        internal static IHostEnvironment env { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            NpgsqlConnectionStringBuilder conn = new NpgsqlConnectionStringBuilder();
            if (env.IsDevelopment())
            {
                conn.Add("User Id", Configuration["PG:Username"]);
                conn.Add("Port", Configuration["PG:Port"]);
                conn.Add("Database", Configuration["PG:DB"]);
                conn.Add("Host", Configuration["PG:Host"]);
            }
            else
            {
                Uri dbUrl = new Uri(Configuration["DATABASE_URL"]);
                string[] userInfo = dbUrl.UserInfo.Split(":");
                conn.Add("User Id", userInfo[0]);
                conn.Add("Password", userInfo[1]);
                conn.Add("Port", dbUrl.Port);
                conn.Add("Database", dbUrl.LocalPath.TrimStart('/'));
                conn.Add("Host", dbUrl.Host);
            }
            pgConnection = conn.ToString();
            services.AddMvc();
            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddDbContext<UserContext>(options => options.UseNpgsql(pgConnection).UseSnakeCaseNamingConvention());
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
            services
                .AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["PG:Host"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SiteKey"])),
                        ValidateAudience = false
                    };
                });
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
