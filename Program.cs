using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logFilePath = builder.Configuration["Logging:File:Path"] ?? "/var/logs";
var rollingInterval = Enum.Parse<RollingInterval>(builder.Configuration["Logging:File:RollingInterval"] ?? "Day");

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(logFilePath, rollingInterval: rollingInterval)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
