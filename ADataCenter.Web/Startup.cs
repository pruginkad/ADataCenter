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

namespace ADataCenter.Web
{
    namespace swag_examples
    {
        public class SingleIncident : IExamplesProvider<Incident>
        {
            public Incident GetExamples()
            {
                return new Incident
                {
                    IncidentName = "Fire",
                    objtype = "CAM",
                    objid = "1",
                    action = "ALARM",
                    IncidentTimestamp = DateTime.UtcNow
                };
            }
        }

        public class MultiIncident : IExamplesProvider<IEnumerable<Incident>>
        {
            public IEnumerable<Incident> GetExamples()
            {
                return new List<Incident>()
            {
                new Incident
                {
                    IncidentName = "Smoke on the water",
                    objtype = "CAM",
                    objid = "1",
                    action = "ALARM",
                    IncidentTimestamp = DateTime.UtcNow
                },
                new Incident
                {
                    IncidentName = "Fire",
                    objtype = "CAM",
                    objid = "2",
                    action = "ALARM",
                    IncidentTimestamp = DateTime.UtcNow
                }
            };
            }
        }
    }
    
    
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
            
            services.AddScoped<IUnitOfWork<Incident>, UnitOfWork>();
            services.AddScoped<IRepository<Incident>, IncidentRepositoryImp>();
            services.AddDbContext<IncidentContext>();

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
                setUpAction.SwaggerEndpoint("/swagger/APISpecification/swagger.json", "Applicant API");
                setUpAction.RoutePrefix = "";

            });

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
