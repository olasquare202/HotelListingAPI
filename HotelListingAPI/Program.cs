using HotelListingAPI;
using HotelListingAPI.Automapper;
using HotelListingAPI.Data;
using HotelListingAPI.IRepository;
using HotelListingAPI.Repository;
using HotelListingAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
);
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
builder.Services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(u =>
{
	
	u.SwaggerDoc("v1",
	new OpenApiInfo()
	{
		Title = "Hotel Listing",
		Version = "v1",
		Description = "This is my HotelListing Web API associated with different countries",
		Contact = new OpenApiContact()
		{
			Email = "olaoluwaesan.dev@gmail.com",
			Name = "Olaoluwa Esan",
			Url = new Uri("https://olasquare202.github.io/Boostrap-V5-Project-with-SASS/")
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

app.UseHttpsRedirection();

app.UseCors("OlaoluwaPolicy");

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

