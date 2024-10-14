namespace GoogleSheetsAPI.Service
{
    public class CsvReaderService
    {
        public static async Task<string[]> ReadCsvFileAsync(string filePath)
        {
            // Kontrollera om filen finns för att undvika fel
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("CSV file not found", filePath);
            }

            return await File.ReadAllLinesAsync(filePath);
        }
    }
}
