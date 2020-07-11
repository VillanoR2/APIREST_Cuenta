using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICuentas_Practica.Controllers;
using APICuentas_Practica.Models;
using APICuentas_Practica.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace APICuentas_Practica
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
            services.Configure<UsuarioDatabaseSettings>(Configuration.GetSection(nameof(UsuarioDatabaseSettings)));

            services.AddSingleton<IUsuarioDatabaseSettings>(sp => {
                var serv = sp.GetRequiredService<IOptions<UsuarioDatabaseSettings>>().Value;
                Console.WriteLine("CONNECTION STRING ANTES: " + serv.ConnectionString);
                serv.ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                Console.WriteLine("CONNECTION STRING DESPUES: " + serv.ConnectionString);
                return serv;
            });

            services.AddSingleton<UserService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
