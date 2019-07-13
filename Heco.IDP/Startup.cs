using Heco.IDP;
using Heco.IDP.Entities;
using Heco.IDP.Services;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Heco.IDP
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HecoUserContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "2617578531631845";
                facebookOptions.AppSecret = "407223b4d6ab1430b8c4fa19fb1dd039";
                facebookOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            }).AddCookie("idsrv.2FA");
            services.AddScoped<IHecoUserRepository, HecoUserRepository>();

            services.AddMvc(options => options.EnableEndpointRouting = false);

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddSigningCredential(LoadCertificateFromStore())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("identityServerDataDBConnectionString"), sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options => 
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("identityServerDataDBConnectionString"), sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddHecoUserStore();
            
            //services.AddIdentityServer();
            //.AddDeveloperSigningCredential(); //TODO <-- Esta configuración es solo para el desarrollo, por lo que debe ser removida antes de hacer el pase a producción
        }

        public X509Certificate2 LoadCertificateFromStore()
        {
            string thumbPrint = "9c72bacfa2af5ee5ad967008434e4dca07f5b36b";

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindByThumbprint,
                    thumbPrint, true);
                if (certCollection.Count == 0)
                {
                    throw new Exception("The specified certificate wasn't fou/nd.");
                }  
                return certCollection[0];
            }
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, HecoUserContext hecoUserContext, ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            configurationDbContext.Database.Migrate();
            configurationDbContext.EnsureSeedDataForContext();

            persistedGrantDbContext.Database.Migrate();

            hecoUserContext.Database.Migrate();
            hecoUserContext.EnsureSeedDataForContext();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

        }
    }
}
