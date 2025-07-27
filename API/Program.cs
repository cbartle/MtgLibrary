var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpClient<Spiff.MtgLibrary.DAL.IExternalAPIService, Spiff.MtgLibrary.DAL.ExternalAPIService>(client => 
{
    client.BaseAddress = new Uri("https://api.scyfall.com/");
});

builder.Services.AddScoped<Spiff.MtgLibrary.DAL.ICardAccess, Spiff.MtgLibrary.DAL.CardAccess>();


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
    await next();
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
