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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts(); // For HTTPS
            }
            // Property => Debug => Enable SSL
            app.UseHttpsRedirection(); // HTTP => HTTPS
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
