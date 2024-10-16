using GoogleSheetsAPI.Models;
using System.Globalization;

namespace GoogleSheetsAPI.Service
{

    public interface ISalesDataExtractor
    {
        object ExtractKWSelectedRows(string[] csvData);
        object ExtractSalesSelectedRows(string[] csvData);
        object ExtractMonthlySalesData(string[] csvData, int monthNumber);

        IEnumerable<SalesData> ExtractSalesBetweenDates(string[] csvData, DateTime start, DateTime end);

    }

    public class SalesDataExtractor : ISalesDataExtractor
    {
        public object ExtractKWSelectedRows(string[] csvData)
        {
            return csvData
                .Skip(3)
                .Take(5)
                .Select(row => row.Split(','))
                .Select(columns => new
                {
                    KEYWORD = columns[0],
                    RANK = columns[1],
                    CVR = columns[3],
                    IMPRESSIONS = columns[4],
                    CLICKS = columns[5],
                    SPEND = columns[6],
                    TOTALSALES = columns[7]
                })
                .OrderBy(row => row.RANK);
        }

        public object ExtractSalesSelectedRows(string[] csvData)
        {
            var totalRow = csvData
                .Select(row => row.Split(';'))
                .FirstOrDefault(columns => columns.Length > 5 && columns[0].Replace("\"", "").Trim().Equals("Totals", StringComparison.OrdinalIgnoreCase));

            if (totalRow == null)
            {
                return new { error = "Total row not found in the CSV file." };
            }

            if (totalRow.Length < 9)
            {
                return new { error = "Insufficient columns in the total row." };
            }

            return new
            {
                TotalSales = totalRow[5].Replace("\"", "").Trim(),
                TotalProducts = totalRow[6].Replace("\"", "").Trim(),
                TotalRefunds = totalRow[8].Replace("\"", "").Trim(),
                Budget = totalRow[14].Replace("\"", "").Trim()
            };
        }

        public object ExtractMonthlySalesData(string[] csvData, int monthNumber)
        {
            var salesData = csvData
                .Skip(1)
                .Select(row => row.Split(';'))
                .Where(columns =>
                {
                    var dateFromString = columns[0].Trim('"');
                    var dateToString = columns[1].Trim('"');

                    string dateFormat = "d/MM/yyyy";
                    if (DateTime.TryParseExact(dateFromString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateFrom) &&
                        DateTime.TryParseExact(dateToString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTo))
                    {
                        return dateFrom.Month <= monthNumber && dateTo.Month >= monthNumber;
                    }
                    return false;
                })
                .Select(columns => new
                {
                    DateFrom = columns[0].Trim('"'),
                    DateTo = columns[1].Trim('"'),
                    OrganicSales = columns[2].Trim('"'),
                    SponsoredSales = columns[3].Trim('"'),
                    Orders = columns[10].Trim('"')
                })
                .ToList();

            if (!salesData.Any())
            {
                return new { error = "No data found for the specified month." };
            }

            return salesData;
        }

        public IEnumerable<SalesData> ExtractSalesBetweenDates(string[] csvData, DateTime start, DateTime end)
        {
            var salesDataList = new List<SalesData>();
            decimal totalSalesSoFar = 0; // Ackumulerad försäljning hittills

            // Börja loopa från andra raden eftersom första raden är rubrikerna
            for (int i = 1; i < csvData.Length; i++)
            {
                var line = csvData[i];
                var columns = line.Split(',');

                if (columns.Length < 4)
                {
                    // Hoppa över rader som inte har tillräckligt med data
                    continue;
                }

                // Försök att parsa datum från första kolumnen
                if (DateTime.TryParseExact(columns[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime currentDate))
                {
                    // Ackumulera försäljning oavsett om datumet är i intervallet
                    if (currentDate <= end)
                    {
                        if (decimal.TryParse(columns[1], out decimal dailySales))
                        {
                            totalSalesSoFar += dailySales;
                        }
                    }

                    // Kontrollera om datumet är inom det angivna intervallet
                    if (currentDate >= start && currentDate <= end)
                    {
                        if (decimal.TryParse(columns[1], out decimal dailySales) &&
                            decimal.TryParse(columns[2], out decimal monthlyGoal))
                        {
                            // Beräkna procentuell avvikelse från målet baserat på total försäljning
                            decimal goalDifferencePercentage = (totalSalesSoFar / monthlyGoal) * 100;

                            // Lägg till försäljningsdata i listan
                            salesDataList.Add(new SalesData
                            {
                                Date = currentDate,
                                DailySales = dailySales,
                                MonthlyGoal = monthlyGoal,
                                GoalDifferencePercentage = goalDifferencePercentage // Procentuell avvikelse från målet
                            });
                        }
                    }
                }
            }

            return salesDataList;
        }

    }
}


