using CensoDemografico.Infra.Context;
using CensoDemografico.Infra.Repositories;
using CensoDemografico.Infra.Repositories.Interfaces;
using CensoDemografico.Infra.Transactions;
using CensoDemografico.Services;
using CensoDemografico.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace CensoDemografico.Api
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services.AddDbContext<CensoDemograficoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BancoCensoDemografico")));

            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Info {
                    Title = "Censo Demografico",
                    Description = "Serviço para cadastramento do Censo demográfico por região",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "Murilo Ramos Parra",
                        Email = "murilo.parra@gmail.com",
                        Url = "https://www.linkedin.com/in/murilo-parra-a599958/"
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPessoaService, PessoaService>();
            services.AddTransient<IPessoaRepository, PessoaRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                UpdateDatabase(app);
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(x=> {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Censo Demográfico v1");
                x.InjectStylesheet("/swagger/css/swagger.css");
                x.InjectOnCompleteJavaScript("/swagger/js/swagger.js");
                x.RoutePrefix = "docs";
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<CensoDemograficoContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
