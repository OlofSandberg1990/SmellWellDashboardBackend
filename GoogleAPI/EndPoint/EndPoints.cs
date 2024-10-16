using GoogleSheetsAPI.Service;
using System.Globalization;

namespace GoogleSheetsAPI.EndPoint
{
    public static class Endpoints
    {
        public static void UseCustomEndpoints(this WebApplication app)
        {
            // Få instansen av ISalesDataExtractor via DI
            var extractor = app.Services.GetRequiredService<ISalesDataExtractor>();

            app.MapGet("/KWRANKING", async (string option) => await GetDataAsync(extractor, option));
            app.MapGet("/SALESRANKING/{month}", async (string month) => await GetSalesDataByMonthAsync(extractor, month));
            app.MapGet("/DETAILEDSALES/{month}", async (string month) => await GetMonthlySalesDataAsync(extractor, month));
            app.MapGet("/FILTEREDSALES/{startDate}/{endDate}", async (string startDate, string endDate) =>
                            await GetFilteredSalesDataAsync(extractor, startDate, endDate));

        }

        private static async Task<IResult> GetDataAsync(ISalesDataExtractor extractor, string option)
        {
            option = string.IsNullOrEmpty(option) ? "1" : option.ToLower();
            var filePath = option.ToLower() switch
            {
                "1" or "zebra" => "csvFiles/kwresearchstest2.csv",
                "2" or "leopard" => "csvFiles/kwresearchstest3.csv",
                _ => null // Om ingen match hittas
            };

            if (filePath == null)
            {
                return Results.BadRequest("Invalid option provided. Please use '1', 'zebra', '2', or 'leopard'.");
            }
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 7)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var selectedRows = extractor.ExtractKWSelectedRows(csvData);
            return Results.Json(selectedRows);
        }

        private static async Task<IResult> GetSalesDataByMonthAsync(ISalesDataExtractor extractor, string monthInput)
        {
            if (!MonthMappingService.TryGetMonthName(monthInput, out var fullMonthName))
            {
                return Results.BadRequest("Invalid month provided. Please use a number, abbreviation, or full month name.");
            }

            var filePath = MonthMappingService.GetSalesFilePath(fullMonthName);

            if (filePath == null)
            {
                return Results.BadRequest("Invalid month provided.");
            }

            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 7)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var selectedRows = extractor.ExtractSalesSelectedRows(csvData);
            return Results.Json(selectedRows);
        }

        private static async Task<IResult> GetMonthlySalesDataAsync(ISalesDataExtractor extractor, string monthInput)
        {
            if (!MonthMappingService.TryGetMonthNumber(monthInput, out var monthNumber))
            {
                return Results.BadRequest("Invalid month provided.");
            }

            var filePath = "csvFiles/DashboardTotalsMonthly.csv";
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 2)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var selectedData = extractor.ExtractMonthlySalesData(csvData, monthNumber);
            return Results.Json(selectedData);
        }

        private static async Task<IResult> GetFilteredSalesDataAsync(ISalesDataExtractor extractor, string startDate, string endDate)
        {
            if (!DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start))
            {
                return Results.BadRequest("Invalid start date. Please use 'yyyy-MM-dd' format.");
            }

            if (!DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end))
            {
                return Results.BadRequest("Invalid end date. Please use 'yyyy-MM-dd' format.");
            }

            if (end < start)
            {
                return Results.BadRequest("End date must be after the start date.");
            }

            var filePath = "csvFiles/september_budget_tracker_minimal.csv";
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 2)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var filteredData = extractor.ExtractSalesBetweenDates(csvData, start, end);
            return Results.Json(filteredData);
        }

    }
}
