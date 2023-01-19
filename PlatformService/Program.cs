using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
var configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.Development.json")
         .Build();
#else
var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.Production.json")
        .Build();
#endif

// Add services to the container.
var conn = configuration.GetConnectionString("PlatformsConn");
Console.WriteLine(conn);

//if (configuration.GetValue<bool>("UseInMemoryDatabase"))
//{
//    Console.WriteLine("--> Using InMemory db");
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseInMemoryDatabase("InMem"));
//}
//else
//{
Console.WriteLine("--> Using MSSQL Server db");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(conn,
   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
//}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> Command Service Endpoint: {configuration["CommandService"]}{configuration["CommandServiceInboundEndpoint"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    PredDb.PrepPopulation(app);
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//builder.Services.AddDbContext<ApplicationDbContext>(opt =>
//opt.UseInMemoryDatabase("InMem"));
