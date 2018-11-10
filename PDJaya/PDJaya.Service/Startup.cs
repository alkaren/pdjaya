using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using PDJaya.Service.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;

namespace PDJaya.Service
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
            var redis = new RedisDB(Configuration.GetConnectionString("RedisCon"));
            var sqlConnectionString = Configuration.GetConnectionString("MySqlCon");

            services.AddDbContext<PDJayaDB>(options =>
                options.UseMySql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("PDJaya.Service")
                )
            );
            services.AddSingleton<RedisDB>((x) => redis);
            services.AddCors();

            // Add framework services.
            //services.AddMvc();
            services.AddMvcCore()
            .AddAuthorization()
            .AddJsonFormatters()
            .AddApiExplorer();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration.GetValue<string>("server:identityurl");
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Configuration.GetValue<string>("server:apiname");
                });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "PDJaya Data API",
                    Version = "v1",
                    Description = "Rest API for accessing data",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "PDJaya Team", Email = "team@PDJaya.com", Url = "http://twitter.com/PDJaya" },
                    License = new License { Name = "For developers only", Url = "http://PDJaya.com" }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.CookieHttpOnly = true;
            });

            //ObjectContainer.Register<RedisDB>(new RedisDB());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
           
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSession();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseIdentity();
            app.UseWebSockets();
           
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller=Default}/{action=Home}/{id?}"
                );
                /*
                routes.MapRoute(
                       name: "api",
                       template: "api/{controller=Home}/{action=Home}/{id?}"
               );*/
            });
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PDJaya Data API V1");
            });
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        }
    }
}
