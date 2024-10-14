using System.Globalization;

namespace GoogleSheetsAPI.Service
{
    public static class MonthMappingService
    {
        private static readonly Dictionary<string, string> MonthMapping = new()
        {
            { "1", "January" }, { "jan", "January" }, { "january", "January" },
            { "2", "February" }, { "feb", "February" }, { "february", "February" },
            { "3", "March" }, { "mar", "March" }, { "march", "March" },
            { "4", "April" }, { "apr", "April" }, { "april", "April" },
            { "5", "May"},
            { "6", "June" }, { "jun", "June" }, { "june", "June" },
            { "7", "July" }, { "jul", "July" }, { "july", "July" },
            { "8", "August" }, { "aug", "August" }, { "august", "August" },
            { "9", "September" }, { "sep", "September" }, { "september", "September" },
            { "10", "October" }, { "oct", "October" }, { "october", "October" },
            { "11", "November" }, { "nov", "November" }, { "november", "November" },
            { "12", "December" }, { "dec", "December" }, { "december", "December" }
        };

        public static bool TryGetMonthName(string input, out string fullMonthName)
        {
            return MonthMapping.TryGetValue(input.Trim().ToLower(), out fullMonthName);
        }

        public static bool TryGetMonthNumber(string input, out int monthNumber)
        {
            if (MonthMapping.TryGetValue(input.Trim().ToLower(), out var monthName))
            {
                monthNumber = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
                return true;
            }

            monthNumber = 0;
            return false;
        }

        public static string GetSalesFilePath(string monthName)
        {
            return monthName switch
            {
                "January" => "csvFiles/MonthlySales/SalesByProducts_2024_Jan.csv",
                "February" => "csvFiles/MonthlySales/SalesByProducts_2024_Feb.csv",
                "March" => "csvFiles/MonthlySales/SalesByProducts_2024_Mar.csv",
                "April" => "csvFiles/MonthlySales/SalesByProducts_2024_Apr.csv",
                "May" => "csvFiles/MonthlySales/SalesByProducts_2024_May.csv",
                "June" => "csvFiles/MonthlySales/SalesByProducts_2024_Jun.csv",
                "July" => "csvFiles/MonthlySales/SalesByProducts_2024_Jul.csv",
                "August" => "csvFiles/MonthlySales/SalesByProducts_2024_Aug.csv",
                "September" => "csvFiles/MonthlySales/SalesByProducts_2024_Sept.csv",
                "October" => "csvFiles/MonthlySales/SalesByProducts_2024_Oct.csv",
                "November" => "csvFiles/MonthlySales/SalesByProducts_2024_Nov.csv",
                "December" => "csvFiles/MonthlySales/SalesByProducts_2024_Dec.csv",
                _ => null
            };
        }
    }
}