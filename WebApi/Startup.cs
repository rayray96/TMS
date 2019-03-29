using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Bootstrap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using WebApi.Configurations;
using Newtonsoft.Json.Serialization;

namespace WebApi
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
            // Using extension method from Bootstrap project.
            services.RegisterApplicationServices("DefaultConnection");
            // Adding and configuring JwtBearer Authentication.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                               .AddJwtBearer(options =>
                               {
                                   options.RequireHttpsMetadata = false; // For real application use true!
                                   options.SaveToken = true;
                                   options.TokenValidationParameters = new TokenValidationParameters
                                   {
                                       // Indicates whether a publisher will be validated when validating a token.
                                       ValidateIssuer = true,
                                       // String representing the publisher.
                                       ValidIssuer = Configuration["Jwt:Issuer"],
                                       // Whether the token consumer will be valid.
                                       ValidateAudience = true,
                                       // Token consumer setting.
                                       ValidAudience = Configuration["Jwt:Audience"],
                                       // Will validation lifetime be.
                                       ValidateLifetime = true,
                                       // Security key installation.
                                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                                       // Security key validation.
                                       ValidateIssuerSigningKey = true,
                                       // The validation parameters have a default clock skew of 5 minutes.
                                       ClockSkew = TimeSpan.Zero
                                   };
                               });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalHost4200", builder => builder.WithOrigins("http://localhost:4200")
                                                              .AllowAnyHeader()
                                                              .AllowAnyMethod()
                                                              .AllowCredentials());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowLocalHost4200"));
            });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //.AddJsonOptions(options =>
            //{
            //    var resolver = options.SerializerSettings.ContractResolver;
            //    if (resolver != null)
            //        (resolver as DefaultContractResolver).NamingStrategy = null;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // The change only affects 204, 205, and 304 status code responses,
            // which aren't allowed to have a body anyways.
            // This change only sets a bool to false on flush,
            // which is checked when IIS is determining if it should set the encoding to chunked or not.
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors("AllowLocalHost4200");
           // app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();
        }
    }
}
