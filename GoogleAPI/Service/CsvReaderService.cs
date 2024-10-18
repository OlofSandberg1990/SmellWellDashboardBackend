namespace GoogleSheetsAPI.Service
{
    // Service responsible for reading CSV files asynchronously
    public class CsvReaderService
    {
        // Method to read all lines from a CSV file asynchronously
        public static async Task<string[]> ReadCsvFileAsync(string filePath)
        {
            // Check if the file exists to prevent errors
            if (!File.Exists(filePath))
            {
                // Throw an exception if the file is not found
                throw new FileNotFoundException("CSV file not found", filePath);
            }

            // Read all lines from the file asynchronously and return them as an array of strings
            return await File.ReadAllLinesAsync(filePath);
        }
    }
}
