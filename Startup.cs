﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManager.Domain.Interfaces.Services;
using StudentsManager.Domain.Models;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using StudentsManager.UI.Services;

namespace StudentsManager.UI
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
            //Used to get response from API
            services.AddTransient<IAPIClient<Student>, APIClient<Student>>();
            services.AddTransient<IAPIClient<Course>, APIClient<Course>>();
            services.AddTransient<IAPIClient<Address>, APIClient<Address>>();

            //Reuses HttpClient to avoid performance problems
            services.AddTransient<HttpClient>();

            //Add our Config object so it can be injected
            services.Configure<AppSettings>(Configuration);
             
            //Add functionality to inject IOptions<T> to read AppSettings
            services.AddOptions();
             
            //Add Mvc
            services.AddMvc();

            //Add Session and MemoryCache to use instead TempData
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {   
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Use Session and MemoryCache to use instead TempData
            app.UseStaticFiles();
            app.UseSession();

            //Add MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}