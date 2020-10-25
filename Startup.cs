using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USF_Health_MVC_EF.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Options;

namespace USF_Health_MVC_EF
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
            Globals.connection = Configuration.GetConnectionString("DefaultConnectionString");

            services.AddHttpContextAccessor();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            .AddAzureAD(options => Configuration.Bind("AzureAD", options));

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
            //});

            services.AddAuthorization(options => {
                options.AddPolicy("usfhealth_athletics",
                        policyBuilder => policyBuilder.RequireClaim("groups",
                        "98dea287-f634-4af9-ad4e-139e14316738"));
            });

            services.AddAuthorization(options => {
                options.AddPolicy("usfhealth_laboratory",
                        policyBuilder => policyBuilder.RequireClaim("groups",
                        "d807ba58-bea1-4b5b-aee9-c625eb12a072"));
            });

            services.AddAuthorization(options => {
                options.AddPolicy("usfhealth_full",
                        policyBuilder => policyBuilder.RequireClaim("groups",
                        "8893fe09-5923-4937-939c-b82bb5553efe"));
            });



            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddMvc();
            services.AddDbContext<USF_Health_MVC_EFContext>(options => options.UseSqlServer(Globals.connection));
            services.AddControllersWithViews();




        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            app.UseDeveloperExceptionPage();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseStatusCodePages();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Index";
                    await next();
                }
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            
        }









































    }
}
