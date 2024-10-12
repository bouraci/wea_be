using EFModels.Data;
using EFModels.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WEA_BE.DTO;
using WEA_BE.Models;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var ConnectionString = builder.Configuration.GetConnectionString("Default");
    var serverVersion = ServerVersion.AutoDetect(ConnectionString);
    options.UseMySql(ConnectionString, serverVersion);
});

var WEACors = "_WEACors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(WEACors,
        policy =>
        {
            policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Book, BookDto>().ReverseMap();
});
string csvPath = builder.Configuration.GetSection("MockDataPath").Get<string>();

builder.Services.AddSingleton(new FilePathOptions { CsvPath = csvPath });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    try
    {
        // Attempt to open a connection to the database
        dbContext.Database.OpenConnection();
        Console.WriteLine("Database connection successful!");
        dbContext.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }

    // migrate if needed, doing this with docker sucks in this stack
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }

}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

//app.UseHttpsRedirection();


app.UseCors(WEACors);

app.UseAuthorization();

app.MapControllers();

app.Run();
