using ADataCenter.Data;
using ADataCenter.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ADataCenter.Web
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
            services.AddControllers();

            services.AddSingleton(arg => new ServiceArgs() { ImagePath = Configuration["ImagePath"] });

            services.AddScoped<IRepository<IncidentFullData>, IncidentRepositoryImp>();

            services.AddScoped<IUnitOfWork<IncidentFullData>, UnitOfWorkIncident>();
            

            services.AddScoped<IUnitOfWorkReport<ReportPage>, UnitOfWorkReport>();

            string cs = Configuration.GetConnectionString("IncidentDatabase");
            services.AddDbContext<IncidentContext>(options =>
                    options.UseNpgsql(cs, o1 => o1.UseNodaTime()));

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Incident>());

            services.AddSwaggerGen(setUpAction =>
            {
                setUpAction.SwaggerDoc("APISpecification", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "IncidentApi",
                    Version = "1.0"
                });
                setUpAction.ExampleFilters();

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //setUpAction.IncludeXmlComments(xmlCommentsFile);
                //setUpAction.AddFluentValidationRules();
            });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(setUpAction =>
            {
                setUpAction.SwaggerEndpoint("/swagger/APISpecification/swagger.json", "Incident API");
                setUpAction.RoutePrefix = "";

            });
            ///Files
            try
            {
                env.WebRootPath = Configuration["ImagePath"];
                System.IO.Directory.CreateDirectory(env.WebRootPath);
            }
            catch(Exception)
            { }
            
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
                     Path.Combine(env.WebRootPath, "")),
                RequestPath = "/MyImages"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            Path.Combine(env.WebRootPath, "")),
                RequestPath = "/MyImages"
            });
            //End files
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
