using AspNetCoreRateLimit;
using HotelListingAPI.Data;
using HotelListingAPI.Model;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Runtime.CompilerServices;
using System.Text;

namespace HotelListingAPI
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(r  => { r.User.RequireUniqueEmail = true; });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }
        public static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            //var secreteKey = Environment.GetEnvironmentVariable("KEY");
            var secreteKey = configuration.GetSection("JWT:KEY").Value;

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,//reject d token if expired
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                    //ValidIssuer = configuration["JWT:Issuer"],  //The commented lines are also correct
                    //ValidAudience = configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secreteKey))//d key value
                                                                                                   //that we get frm d Environment i.e secreteKey.
                                                                                                   //It is hashed through this line of code
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"]))
                };
            });
            return services;
        }
        public static void ConfigureGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    //context.Response.StatusCode = 500;
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error($"Something went wrong in the {contextFeature.Error}");

                        await context.Response.WriteAsync(new GlobalErrorHangling
                        {
                            statusCode = context.Response.StatusCode,
                            message = "Internal Server Error. Please Try Again Later."
                        }.ToString());
                    }
                });
            });
        }

        public static void ConfigureAPIVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
        //How to configure global Cache for all d APIs in d app
        //public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        //{
        //    services.AddResponseCaching();
        //    services.AddHttpCacheHeaders(
        //        (experationOpt) =>
        //        {
        //            experationOpt.MaxAge = 120;
        //            experationOpt.CacheLocation = CacheLocation.Private;
        //        },
        //        (validateOpt) =>
        //        {
        //            validateOpt.MustRevalidate = true;
        //        }
        //        );
        //} //If I uncomment Dis mtd above & it middleware in Program.cs"app.UseHttpCacheHeaders();", it will override d below mtd "RateLimit"
        //How to configure RateLimit(i.e to avoid over calling our APIs)
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {//It only working in HotelController bcos Cache is working in CountryController
                new RateLimitRule//This is just a global rule for all end points in this app
                {
                    Endpoint = "*",//for All APIs in this app, you are limited to one call per second
                    Limit = 1,//one call
                    Period = "5s"//one second
                },
                // new RateLimitRule
                //{
                //    Endpoint = "*",//you can specify an API and set d limit call as well as d period
                //    Limit = 20,//one call
                //    Period = "1000s"//depending on what you want
                //    //N.B: YOU CAN HAVE MULTIPLE RULES
                //}
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();//These codes
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();//only support d dependency I installed
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();// "AspNetCoreRateLimit"
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();//I added this line to make d latest version(5) dependency work. "AspNetCoreRateLimit"
        }//Diff. dependency with different codes
    }
}
