using Microsoft.EntityFrameworkCore;
using task;
using task.Data;
using task.Services;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DellinDictionaryDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<TerminalImportService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
