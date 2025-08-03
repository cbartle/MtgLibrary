using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Headers;

namespace Spiff.MtgLibrary.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            builder.Services.AddHttpClient<Spiff.MtgLibrary.DAL.IExternalAPIService, Spiff.MtgLibrary.DAL.ExternalAPIService>(client =>
            {
                client.BaseAddress = new Uri("https://api.scryfall.com/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; Spiff.MtgLibrary/1.0)");
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
        }
    }
}
