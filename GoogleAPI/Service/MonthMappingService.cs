using System.Globalization;

namespace GoogleSheetsAPI.Service
{
    // Static class responsible for mapping month names and numbers, and retrieving file paths based on the month.
    public static class MonthMappingService
    {
        // Dictionary that maps different representations of months (numbers, abbreviations, and full names) to their full month names.
        private static readonly Dictionary<string, string> MonthMapping = new()
        {
            { "1", "January" }, { "jan", "January" }, { "january", "January" },
            { "2", "February" }, { "feb", "February" }, { "february", "February" },
            { "3", "March" }, { "mar", "March" }, { "march", "March" },
            { "4", "April" }, { "apr", "April" }, { "april", "April" },
            { "5", "May" },
            { "6", "June" }, { "jun", "June" }, { "june", "June" },
            { "7", "July" }, { "jul", "July" }, { "july", "July" },
            { "8", "August" }, { "aug", "August" }, { "august", "August" },
            { "9", "September" }, { "sep", "September" }, { "september", "September" },
            { "10", "October" }, { "oct", "October" }, { "october", "October" },
            { "11", "November" }, { "nov", "November" }, { "november", "November" },
            { "12", "December" }, { "dec", "December" }, { "december", "December" }
        };

        // Method that tries to get the full month name from an input string (can be a number, abbreviation, or full name).
        public static bool TryGetMonthName(string input, out string fullMonthName)
        {
            return MonthMapping.TryGetValue(input.Trim().ToLower(), out fullMonthName);
        }

        // Method that tries to get the month number from an input string (can be a number, abbreviation, or full name).
        public static bool TryGetMonthNumber(string input, out int monthNumber)
        {
            // Retrieve the full month name from the input using the dictionary
            if (MonthMapping.TryGetValue(input.Trim().ToLower(), out var monthName))
            {
                // Parse the month name to get the corresponding month number
                monthNumber = DateTime.ParseExact(monthName, "MMMM", CultureInfo.InvariantCulture).Month;
                return true;
            }

            monthNumber = 0; // Default if no valid month is found
            return false;
        }

        // Method to retrieve the file path of the sales CSV file for a specific month based on its full name.
        public static string GetSalesFilePath(string monthName)
        {
            // Use a switch expression to map the month name to its corresponding file path
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
                "October" => "csvFiles/MonthlySales/SalesByProducts_2023_Oct.csv",
                "November" => "csvFiles/MonthlySales/SalesByProducts_2023_Nov.csv",
                "December" => "csvFiles/MonthlySales/SalesByProducts_2023_Dec.csv",
                _ => null // Return null if no valid month name is provided
            };
        }
    }
}
