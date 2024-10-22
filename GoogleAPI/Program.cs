using GoogleSheetsAPI.EndPoint;
using GoogleSheetsAPI.Service;

namespace GoogleAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register custom services for keyword ranking and sales data
            builder.Services.AddSingleton<IKeywordRankingService, KeywordRankingService>();
            builder.Services.AddSingleton<ISalesDataService, SalesDataService>();

            // Configure CORS to allow specific origins (for API calls from certain URLs)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.WithOrigins(
                        "https://googlesheetsapi-b4e4bdh9a0fpakg0.westeurope-01.azurewebsites.net/",
                        "http://localhost:4200", "https://thankful-sand-08fac1a03.5.azurestaticapps.net/")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Register CsvReaderService as a singleton service for CSV file reading
            builder.Services.AddSingleton<CsvReaderService>();

            var app = builder.Build();

            // Redirect root URL to Swagger, and exclude this endpoint from Swagger documentation
            app.MapGet("/", () => Results.Redirect("/swagger"))
               .ExcludeFromDescription();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");
            app.MapControllers();
            app.UseCustomEndpoints();
            app.Run();
        }
    }
}
