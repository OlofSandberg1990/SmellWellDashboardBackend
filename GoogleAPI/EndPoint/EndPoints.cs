using GoogleSheetsAPI.Service;
using GoogleSheetsAPI.Models;
using System.Globalization;

namespace GoogleSheetsAPI.EndPoint
{
    public static class Endpoints
    {
        // Configure and map custom endpoints for the web application
        public static void UseCustomEndpoints(this WebApplication app)
        {
            // Get instances of services through Dependency Injection
            var keywordService = app.Services.GetRequiredService<IKeywordRankingService>();
            var salesService = app.Services.GetRequiredService<ISalesDataService>();

            // Map GET endpoints to their respective handler methods
            app.MapGet("/KWRANKING", async (string option) => await GetKeywordDataAsync(keywordService, option));
            app.MapGet("/SALESRANKING/{month}", async (string month) => await GetSalesDataByMonthAsync(salesService, month));
            app.MapGet("/DETAILEDSALES/{month}", async (string month) => await GetMonthlySalesDataAsync(salesService, month));
            app.MapGet("/FILTEREDSALES/{startDate}/{endDate}", async (string startDate, string endDate) =>
                            await GetFilteredSalesDataAsync(salesService, startDate, endDate));
        }

        // Handle requests for keyword ranking data
        private static async Task<IResult> GetKeywordDataAsync(IKeywordRankingService keywordService, string option)
        {
            // Set default option and determine file path based on the option
            option = string.IsNullOrEmpty(option) ? "1" : option.ToLower();
            var filePath = option switch
            {
                "1" or "zebra" => "csvFiles/kwresearchstest2.csv",
                "2" or "leopard" => "csvFiles/kwresearchstest3.csv",
                _ => null
            };

            // Return bad request if invalid option is provided
            if (filePath == null)
            {
                return Results.BadRequest("Invalid option provided. Please use '1', 'zebra', '2', or 'leopard'.");
            }

            // Read CSV file and process data
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 7)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var selectedRows = keywordService.GetKeywordRanking(csvData);
            return Results.Json(selectedRows); // Returns KeywordRanking model
        }

        // Handle requests for sales data by month
        private static async Task<IResult> GetSalesDataByMonthAsync(ISalesDataService salesService, string monthInput)
        {
            // Validate and convert month input
            if (!MonthMappingService.TryGetMonthName(monthInput, out var fullMonthName))
            {
                return Results.BadRequest("Invalid month provided. Please use a number, abbreviation, or full month name.");
            }

            var filePath = MonthMappingService.GetSalesFilePath(fullMonthName);

            if (filePath == null)
            {
                return Results.BadRequest("Invalid month provided.");
            }

            // Read and process CSV data
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 7)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var selectedRows = salesService.GetSalesSummary(csvData);
            return Results.Json(selectedRows); // Returns SalesSummary model
        }

        // Handle requests for detailed monthly sales data
        private static async Task<IResult> GetMonthlySalesDataAsync(ISalesDataService salesService, string monthInput)
        {
            // Validate and convert month input
            if (!MonthMappingService.TryGetMonthNumber(monthInput, out var monthNumber))
            {
                return Results.BadRequest("Invalid month provided.");
            }

            // Read and process CSV data
            var filePath = "csvFiles/DashboardTotalsMonthly.csv";
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 2)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var selectedData = salesService.GetMonthlySalesData(csvData, monthNumber);
            return Results.Json(selectedData); // Returns MonthlySalesData model
        }

        // Handle requests for filtered sales data within a date range
        private static async Task<IResult> GetFilteredSalesDataAsync(ISalesDataService salesService, string startDate, string endDate)
        {
            // Validate and parse start and end dates
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

            // Read and process CSV data
            var filePath = "csvFiles/september_budget_tracker_minimal.csv";
            var csvData = await CsvReaderService.ReadCsvFileAsync(filePath);

            if (csvData.Length < 2)
            {
                return Results.BadRequest("CSV file does not contain enough data.");
            }

            var filteredData = salesService.GetFilteredSalesData(csvData, start, end);
            return Results.Json(filteredData); // Returns IEnumerable<SalesData>
        }
    }
}
