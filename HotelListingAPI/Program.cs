using AspNetCoreRateLimit;
using HotelListingAPI;
using HotelListingAPI.Automapper;
using HotelListingAPI.Data;
using HotelListingAPI.IRepository;
using HotelListingAPI.Repository;
using HotelListingAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Security.Cryptography.Xml;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
);
//builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimiting();
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCaching();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
//Add Automapper
builder.Services.AddAutoMapper(typeof(Mapping));
//Add IdentityUser


builder.Services.ConfigureJWT(Configuration);
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();//I configure d IdentityUser in
									 //ServiceExtensions class then registered it here
									 //To avoid much codes in this Program.cs file
builder.Services.ConfigureAPIVersioning();

//Configure CORS Policy(i.e Cross Origin Resource Shearing)
builder.Services.AddCors(e =>
{
	e.AddPolicy("OlaoluwaPolicy", builder =>
	builder.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());
});
//Logger configuration
Log.Logger = new LoggerConfiguration().WriteTo.File(path: "c:\\hotellistings\\logs\\log-.txt",
	outputTemplate: "{Timestamp:yyyy-mm-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:1j}{NewLine}{Exception}",
	rollingInterval: RollingInterval.Day,
	restrictedToMinimumLevel: LogEventLevel.Information
	).CreateLogger();
//try
//{
//	Log.Information("Application is starting");
//}
//catch (Exception ex)
//{

//	Log.Fatal(ex, "Application Failed to start");
//}
//finally
//{
//	Log.CloseAndFlush();
//}
builder.Host.UseSerilog();
builder.Services.AddControllers(config => //This is how to do Global caching configuration
{
	config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
	{
		Duration = 120
	});
}).AddNewtonsoftJson(opt =>
opt.SerializerSettings.ReferenceLoopHandling =
Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(u =>
{
	
	u.SwaggerDoc("v1",
	new OpenApiInfo()
	{
		Title = "Hotel Listing",
		Version = "v1",
		//Description = "This is my HotelListing Web API associated with different countries",
		Description = "This Robust Hotel Listing web API was built using ASP.NET Core 6.\r\n\r\n with Entity Framework and Enterprise Level Design Patterns.\r\n\r\n which allows only Authorized Users to access the API and perform CRUD operations.\r\n\r\n The following features were implemented:\r\n\r\n Logging events & errors, Global Error Handling, CORS Policy,\r\n\r\n JWT Bearer, C# Identity, Rate Limiting, API Documentation using SwaggerUI,\r\n\r\n API Versionning, Pagination, Global Caching for all APIs, Unit Of Work, Generic Repository Pattern and lots more.\r\n\r\n CHECK MY PORTFOLIO VIA THE LINK BELOW\r\n\r\n Feel free to Register and Login, then use the Token generated to access all the APIs\r\n\r\nTechnologies Used: C#, EFCore, ASP.NETCore, MSSQL Server, SwaggerUI(for Bearer Token), Postman and GitHub.",

		Contact = new OpenApiContact()
		{
			Email = "olaoluwaesan.dev@gmail.com",
			Name = "Check MY Portfolio",
			Url = new Uri("https://olasquare202.github.io/Boostrap-V5-Project-with-SASS/")
			
		}
	});
	u.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Enter 'Bearer' give space then paste the bearer Token.\r\n\r\n" +
					  "Example: \"Bearer eyjoo354bdjkhvq83esno\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	u.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
		   {
			Reference = new OpenApiReference
			{
				Type = ReferenceType.SecurityScheme,
				Id = "Bearer"
			}
		},
			new String[]{ }
		}

	});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options => 
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Listing Web API v1")
		

	);
}

app.ConfigureGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("OlaoluwaPolicy");

app.UseResponseCaching();

//app.UseHttpCacheHeaders();

app.UseIpRateLimiting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


//using Serilog;
//using Serilog.Events;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
////Logger configuration
//Log.Logger = new LoggerConfiguration().WriteTo.File(path: "c:\\hotellistings\\logs\\log-.txt",
//    outputTemplate: "{Timestamp:yyyy-mm-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:1j}{NewLine}{Exception}",
//    rollingInterval: RollingInterval.Day,
//    restrictedToMinimumLevel: LogEventLevel.Information
//    ).CreateLogger();
//try
//{
//    Log.Information("Application is statrting");

//    builder.Services.AddControllers();
//    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//    builder.Services.AddEndpointsApiExplorer();
//    builder.Services.AddSwaggerGen();

//    var app = builder.Build();

//    // Configure the HTTP request pipeline.
//    if (app.Environment.IsDevelopment())
//    {
//        app.UseSwagger();
//        app.UseSwaggerUI();
//    }

//    app.UseHttpsRedirection();

//    app.UseAuthorization();

//    app.MapControllers();

//    app.Run();
//}
//catch (Exception ex)
//{

//    Log.Fatal(ex, "Application Failed to start");
//}
//finally
//{
//    Log.CloseAndFlush();
//}

