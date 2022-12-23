using Assignment4_Hearthstone.Models;
using Assignment4_Hearthstone.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<CardService>();
builder.Services.AddSingleton<CardTypeService>();
builder.Services.AddSingleton<ClassService>();
builder.Services.AddSingleton<RarityService>();
builder.Services.AddSingleton<SetService>();
builder.Services.AddControllers();

builder.Host.ConfigureLogging(log =>
{
    log.ClearProviders();
    log.AddConsole();
});

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
