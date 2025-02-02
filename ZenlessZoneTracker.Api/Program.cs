using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using ZenlessZoneTracker.Application.Interfaces;
using ZenlessZoneTracker.Application.Services;
using ZenlessZoneTracker.Infrastructure.External.HoYoApi;

var builder = WebApplication.CreateBuilder(args);

var jsonSettings = new JsonSerializerSettings
{
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy()
    }
};

builder.Services.AddTransient<IFetchGachaService, FetchGachaService>();

builder.Services
    .AddRefitClient<IGachaApi>(
        new RefitSettings(
            new NewtonsoftJsonContentSerializer(jsonSettings)
        ))
    .ConfigureHttpClient(client => { client.BaseAddress = new Uri("https://public-operation-nap-sg.hoyoverse.com"); });


builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddSwaggerGen()
    .AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();