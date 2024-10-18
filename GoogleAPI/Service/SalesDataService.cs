using GoogleSheetsAPI.Models;
using System.Globalization;

namespace GoogleSheetsAPI.Service
{
    // Interface that defines the methods to retrieve sales data
    public interface ISalesDataService
    {
        SalesSummary GetSalesSummary(string[] csvData);
        IEnumerable<MonthlySalesData> GetMonthlySalesData(string[] csvData, int monthNumber);
        IEnumerable<SalesData> GetFilteredSalesData(string[] csvData, DateTime start, DateTime end);
    }

    // Class that implements the sales data service, responsible for extracting sales data from CSV files
    public class SalesDataService : ISalesDataService
    {
        // Method to get the sales summary from a CSV file
        public SalesSummary GetSalesSummary(string[] csvData)
        {
            // Find the row where 'Totals' is present in the first column
            var totalRow = csvData
                .Select(row => row.Split(';'))
                .FirstOrDefault(columns => columns.Length > 5 &&
                    columns[0].Replace("\"", "").Trim().Equals("Totals", StringComparison.OrdinalIgnoreCase));

            // If no row was found, return null (better error handling can be added)
            if (totalRow == null)
            {
                return null; // Handle this error better if needed
            }

            // Return the sales summary by extracting values from the identified row
            return new SalesSummary
            {
                TotalSales = totalRow[5].Replace("\"", "").Trim(),
                TotalProducts = totalRow[6].Replace("\"", "").Trim(),
                TotalRefunds = totalRow[8].Replace("\"", "").Trim(),
                Budget = totalRow[14].Replace("\"", "").Trim()
            };
        }

        // Method to get monthly sales data based on the provided month number
        public IEnumerable<MonthlySalesData> GetMonthlySalesData(string[] csvData, int monthNumber)
        {
            // Filter rows where the date range includes the specified month
            return csvData
                .Skip(1) // Skip header row
                .Select(row => row.Split(';'))
                .Where(columns =>
                {
                    var dateFromString = columns[0].Trim('"');
                    var dateToString = columns[1].Trim('"');

                    string dateFormat = "d/MM/yyyy";
                    // Parse date range and check if the month falls within the range
                    if (DateTime.TryParseExact(dateFromString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateFrom) &&
                        DateTime.TryParseExact(dateToString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTo))
                    {
                        return dateFrom.Month <= monthNumber && dateTo.Month >= monthNumber;
                    }
                    return false;
                })
                // Select relevant data into MonthlySalesData objects
                .Select(columns => new MonthlySalesData
                {
                    DateFrom = columns[0].Trim('"'),
                    DateTo = columns[1].Trim('"'),
                    OrganicSales = columns[2].Trim('"'),
                    SponsoredSales = columns[3].Trim('"'),
                    Orders = columns[10].Trim('"')
                });
        }

        // Method to get filtered sales data between a start date and an end date
        public IEnumerable<SalesData> GetFilteredSalesData(string[] csvData, DateTime start, DateTime end)
        {
            var salesDataList = new List<SalesData>(); // List to store the filtered sales data
            decimal totalSalesSoFar = 0; // Variable to keep track of accumulated sales

            // Iterate over each row in the CSV (skipping the header row)
            for (int i = 1; i < csvData.Length; i++)
            {
                var line = csvData[i];
                var columns = line.Split(',');

                if (columns.Length < 4)
                {
                    continue; // Skip rows that don't have enough data
                }

                // Parse the date from the first column
                if (DateTime.TryParseExact(columns[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime currentDate))
                {
                    // Accumulate sales data if the date is before the end date
                    if (currentDate <= end)
                    {
                        if (decimal.TryParse(columns[1], out decimal dailySales))
                        {
                            totalSalesSoFar += dailySales;
                        }
                    }

                    // Check if the current date is within the specified date range
                    if (currentDate >= start && currentDate <= end)
                    {
                        if (decimal.TryParse(columns[1], out decimal dailySales) &&
                            decimal.TryParse(columns[2], out decimal monthlyGoal))
                        {
                            // Calculate the percentage difference from the goal
                            decimal goalDifferencePercentage = (totalSalesSoFar / monthlyGoal) * 100;

                            // Add the sales data to the list
                            salesDataList.Add(new SalesData
                            {
                                Date = currentDate,
                                DailySales = dailySales,
                                MonthlyGoal = monthlyGoal,
                                GoalDifferencePercentage = goalDifferencePercentage
                            });
                        }
                    }
                }
            }

            return salesDataList; // Return the filtered sales data
        }
    }
}
