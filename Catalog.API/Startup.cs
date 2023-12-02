using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.Application.Emails;
using Catalog.API.Mediation;
using Catalog.API.Processing;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Configuration.Processing;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Catalog.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private static Serilog.ILogger _logger;
        public Startup(IWebHostEnvironment env)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables("Library_")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IEmailService>(provider =>
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var smtpServer = smtpSettings["Server"];
                var smtpPort = int.Parse(smtpSettings["Port"]);
                var smtpEnableSsl = bool.Parse(smtpSettings["EnableSsl"]);
                var smtpUsername = smtpSettings["Username"];
                var smtpPassword = smtpSettings["Password"];

                return new SmtpEmailService(smtpServer, smtpPort, smtpEnableSsl, smtpUsername, smtpPassword);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               var appSettings = _configuration.GetSection("AppSettings");
               var token = appSettings["Token"];
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = false,
                   ValidIssuer = "your_publisher",
                   ValidAudience = "your_public",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token)),
                   ClockSkew = TimeSpan.Zero
               };
           });

            services.AddAuthorization();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineLibrary-Catalog", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("Cors", builder =>
                {

                    builder.WithOrigins(new string[]{
                    "http://localhost:8080",
                    "https://localohost:8080",
                    "http://127.0.0.1:8080",
                    "https://127.0.0.1:8080",
                    "https://localhost:5001",
                    "https://127.0.0.1:5001",
                    "https://192.168.100.22",
                    "http://192.168.100.22"

                }).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            var connectionStrings = _configuration.GetSection("ConnectionStrings");
            var connection = connectionStrings["DefaultConnection"];

            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connection));
            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new CatalogAutofacModule());
            containerBuilder
             .RegisterAssemblyTypes(new Assembly[]{
                typeof(Startup).Assembly})
             .AsClosedTypesOf(typeof(IValidator<>))
             .InstancePerLifetimeScope();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var container = app.ApplicationServices.GetAutofacRoot();
            app.UseCors(builder =>
               builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            InitializeModules(container);
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineLibrary v1");
                    c.RoutePrefix = string.Empty;
                });

            }
            app.UseFileServer(enableDirectoryBrowsing: true);
            app.UseRouting();
            app.UseCors("Cors");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
            app.UseMvc();
        }

        private void InitializeModules(ILifetimeScope container)
        {
            var httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            var executionContextAccessor = new ExecutionContextAccessor(httpContextAccessor);
            var connectionStrings = _configuration.GetSection("ConnectionStrings");
            var connection = connectionStrings["DefaultConnection"];
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpServer = smtpSettings["Server"];
            var smtpPort = int.Parse(smtpSettings["Port"]);
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];

            CatalogStartup.Initialize(
                 connection,
                 smtpServer,
                 smtpPort,
                 enableSsl,
                 smtpUsername,
                 smtpPassword,
                executionContextAccessor,
               _logger,
                null
                );
        }
    }
}
