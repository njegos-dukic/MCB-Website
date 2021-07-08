using AspNetCoreRateLimit;
using Core.Entity;
using Core.Managers;
using Core.Util;
using MasterCraftBreweryAPI.Authentication;
using MasterCraftBreweryAPI.AutoMapper;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MasterCraftBreweryAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string apiVersion = Configuration["ApiVersion"];

            services.AddHttpContextAccessor();

            services.AddDbContext<MasterCraftBreweryContext>(
                  options => options.UseMySql(Configuration.GetConnectionString("Database")
            ));

            // Register managers as services
            services.AddScoped<IFileManager, LocalFileManager>(x => new LocalFileManager(Configuration["FileStorageLocation"]));
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IMenuManager, MenuManager>();
            services.AddScoped<ILoginManager, LoginManager>();
            services.AddScoped<IGalleryManager, GalleryManager>();
            services.AddScoped<IQuoteManager, QuoteManager>();
            services.AddScoped<IAdministratorManager, AdministratorManager>();
            services.AddScoped<IEventManager, EventManager>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IShopManager, ShopManager>();
            services.AddScoped<IContactManager, ContactManager>();
            services.AddScoped<IApiKeyManager, ApiKeyManager>(provider =>
            {
                IHttpContextAccessor httpContext = provider.GetRequiredService<IHttpContextAccessor>();
                string apiKey = httpContext.HttpContext.Request.Headers["X-API-KEY"];
                return new ApiKeyManager(apiKey, ActivatorUtilities.CreateInstance<MasterCraftBreweryContext>(provider));
            });

            services.AddScoped<IAuthorizationHandler, TokenAuthorizationHandler>();

            services.AddSingleton(ctx => MappingConfiguration.CreateMapping());

            services.AddControllers(x => { x.UseGeneralRoutePrefix($"api/v{apiVersion}"); });

            services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = "MasterCraftBrewery API", Version = apiVersion });
                 string xmlFile = $"MasterCraftBreweryAPI.xml";
                 c.IncludeXmlComments(xmlFile);
                 c.OperationFilter<AddRequiredHeaderParameter>();
             });

            // Needed to load configuration from appsettings.json
            services.AddOptions();

            // Needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            // Load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            // Load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            // Inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddCors(options =>
            {
                options.AddPolicy("Wildcard",
                    builder =>
                    {
                        builder.WithOrigins("*")
                                .WithHeaders("x-api-key", "content-type", "authorize")
                                .WithMethods("POST", "PUT", "DELETE", "GET");
                    });
            });

            services.AddAuthentication()
                    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Authentication.Authentication.ApiKeyScheme, null);

            services.AddAuthorization(options =>
                     options.AddPolicy("TokenRequired",
                     policy => policy.Requirements.Add(new TokenAuthorizationRequirement())));
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string apiVersion = Configuration["ApiVersion"];

            app.UseIpRateLimiting();
#if USE_DEVELOPMENT
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
        }
#endif
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{apiVersion}/swagger.json", "MasterCraftBrewery API");
            });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
