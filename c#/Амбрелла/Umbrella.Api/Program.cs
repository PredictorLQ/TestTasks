using Umbrella.Api.Extensions;
using Umbrella.Application.Extensions;
using Umbrella.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddSwaggerConfiguration();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.Services.MigrateDatabaseAsync();

app.Run();

