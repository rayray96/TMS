using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bootstrap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;

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
            // Is that really need???
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
            // In ASP.NET Core 2.2, we released a new Server which runs inside IIS for Windows scenarios.
            // When sending the XMLHttpRequest, there is a preflight OPTIONS request which returns a status code of 204.
            // This was incorrectly handled by the IIS server, returning an invalid response to the client.
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
