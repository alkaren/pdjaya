using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PDJaya.Identity.Helpers;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PDJaya.Identity
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
            var sqlConnectionString = Configuration.GetConnectionString("MySqlCon");

            services.AddDbContext<PDJayaDB>(options =>
                options.UseMySql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("PDJaya.Identity")
                )
            );
            //my user repository
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc();
            // configure identity server with in-memory stores, keys, clients and resources
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                .AddProfileService<ProfileService>();
            //Inject the classes we just created
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            //added new
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            /*
            IdentityServerAuthenticationOptions identityServerValidationOptions = new IdentityServerAuthenticationOptions
            {
                //move host url into appsettings.json
                Authority = "http://localhost:50000/",
                ApiSecret = "secret",
                ApiName = "my.api.resource",
                AutomaticAuthenticate = true,
                SupportedTokens = SupportedTokens.Both,

                // required if you want to return a 403 and not a 401 for forbidden responses
                AutomaticChallenge = true,

                //change this to true for SLL
                RequireHttpsMetadata = false
            };

            app.UseIdentityServerAuthentication(identityServerValidationOptions);
            */


        }
    }
}
