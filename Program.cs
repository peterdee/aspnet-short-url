using dotenv.net;
using aspnet_short_url.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// set application port
var port = Environment.GetEnvironmentVariable("PORT");
if (port == null || port == "")
{
    port = "4500";
}
builder.WebHost.UseUrls(["http://localhost:" + port]);

builder.Services.AddSingleton<ShortUrlService>();
builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        // TODO: fix Swagger
    });
}

app.Run();
