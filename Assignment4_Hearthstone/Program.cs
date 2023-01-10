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

// Using a Scope to call the Create() methods from the Services to seed database
// Found inspiration for this code at https://peakup.org/blog/asp-net-core-dependency-injection-and-service-lifetimes/
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    var seedCards = service.GetRequiredService<CardService>();
    var seedCardTypes = service.GetRequiredService<CardTypeService>();
    var seedClasses = service.GetRequiredService<ClassService>();
    var seedRarities = service.GetRequiredService<RarityService>();
    var seedSets = service.GetRequiredService<SetService>();

    seedCards.CreateCards();
    seedCardTypes.CreateCardTypes();
    seedClasses.CreateClasses();
    seedRarities.CreateRarities();
    seedSets.CreateSets();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.Run();
