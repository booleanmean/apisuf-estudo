using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;


using System.Reflection;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.OpenApi.Models;


namespace APICET_BKEND
{
    public class Startup
    {
     
     
        private static string GetPathOfXmlFromAssembly() => Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
     
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICET_BKEND", Version = "v1" });  
                c.IncludeXmlComments(GetPathOfXmlFromAssembly());              
            });


            //orig:
            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICET_BKEND", Version = "v1" });                
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment() )
           //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APICET_BKEND v1"));
            //}


            app.UseDefaultFiles();
            app.UseStaticFiles();


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
