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
            builder.Services.AddSingleton<ISalesDataExtractor, SalesDataExtractor>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.WithOrigins(
                        "https://googlesheetsapi-b4e4bdh9a0fpakg0.westeurope-01.azurewebsites.net/",
                        "http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            // Registrera CsvReaderService
            builder.Services.AddSingleton<CsvReaderService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");
            app.MapControllers();
            app.UseCustomEndpoints();
            app.Run();
        }
    }
}
