using System;
using Boardz.Id.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boardz.Id
{
    public class Startup
    {
        protected IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            this.Configuration = configBuilder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
            var idSrvConfig = Configuration.GetSection("IdSrv");
            services.AddIdentityServer()
                .AddTestUsers(InMemory.GetUsers(idSrvConfig))
                .AddInMemoryClients(InMemory.GetClients(idSrvConfig))
                .AddInMemoryIdentityResources(InMemory.GetIdentityResources())
                .AddInMemoryApiResources(InMemory.GetApiResources(idSrvConfig))
                .AddDeveloperSigningCredential();
            services.AddCors();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            
            loggerFactory.AddDebug();
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseIdentityServer();
            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}
