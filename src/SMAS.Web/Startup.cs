using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SMAS.Data;
using SMAS.Entities;
using SMAS.Web.Core.Configuration;
using SMAS.Web.Core.Services;
using OrangeJetpack.Core.Web.Utilities;
using OrangeJetpack.Services.Client.Messaging;
using OrangeJetpack.Services.Client.Storage;
using SMAS.Web.Core.Extensions;
using SMAS.Web.Core.Middleware;

namespace SMAS.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                //builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<IdentityOptions>(option =>
            {
                option.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                option.Cookies.ApplicationCookie.AutomaticChallenge = true;
                option.Cookies.ApplicationCookie.AuthenticationScheme = "Cookie";
            });
            services.AddCors();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;

                    // Cookie settings
                    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(90);
                    options.Cookies.ApplicationCookie.LoginPath = "/account/login";
                    options.Cookies.ApplicationCookie.LogoutPath = "/account/logoff";


                    // User settings
                    options.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<DataContext, int>()
                .AddDefaultTokenProviders();

            services
                .AddMvc(i =>
                {
                    i.Conventions.Add(new FeatureConvention());
                    i.Conventions.Add(new FromBodyConvention());
                })
                .AddRazorOptions(options =>
                {
                    // {0} - Action Name
                    // {1} - Controller Name
                    // {2} - Area Name
                    // {3} - Feature Name
                    options.AreaViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{3}/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{3}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Features/Shared/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/Shared/{0}.cshtml");

                    // replace normal view location entirely
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");

                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

                });



            services.AddRouting(options => options.LowercaseUrls = true);


            services.AddKendo();

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();



            services.AddSingleton<IDataContextFactory>(new DataContextFactory(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IAppServices, AppServices>();

            services.AddSingleton<IMessageService>(new MessageService(
                Configuration["AppSettings:ProjectKey"],
                Configuration["AppSettings:ProjectToken"],
                Configuration["AppSettings:EmailSender"],
                Configuration["AppSettings:SiteTitle"]));

            services.AddSingleton<IStorageService>(new AzureBlobService(
                Configuration["AppSettings:ProjectKey"],
                Configuration["AppSettings:ProjectToken"],
                Configuration.GetConnectionString("StorageConnection")
            ));


            services.AddScoped<ViewRender, ViewRender>();
            services.AddAuthentication(options =>
            {
                options.SignInScheme = "ServerCookie";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DataContext dc)
        {
            var secretKey = Configuration["Api:ApiJwtAuthenticationKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var issuer = Configuration["Api:Issuer"];
            var audience = Configuration["Api:Audience"];

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
                        builder
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin()
                       .AllowCredentials()
                );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseDeveloperExceptionPage();

                //app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameter,
                AuthenticationScheme = "Bearer"
            });

            app.UseApiKeyMiddleware(Configuration["Api:ApiKey"]);

            var tokenOption = new TokenProviderOptions
            {
                Path = "/token",
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            app.UseTokenProviderMiddleware(tokenOption);



            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });

            app.UseKendo(env);

            dc.Database.Migrate();
        }
    }
}
