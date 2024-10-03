using EFModels.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
    options.UseMySQL(ConnectionString);
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