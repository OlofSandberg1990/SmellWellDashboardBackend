using GoogleSheetsAPI.Models;

namespace GoogleSheetsAPI.Service
{
    // Interface that defines the method to retrieve keyword rankings from CSV data
    public interface IKeywordRankingService
    {
        IEnumerable<KeywordRanking> GetKeywordRanking(string[] csvData);
    }

    // Implementation of the keyword ranking service responsible for extracting keyword ranking data from CSV files
    public class KeywordRankingService : IKeywordRankingService
    {
        // Method to get keyword ranking from a CSV file
        public IEnumerable<KeywordRanking> GetKeywordRanking(string[] csvData)
        {
            // Skip the first 3 rows, take the next 5 rows, split each row by commas,
            // then create KeywordRanking objects from the columns and order them by rank
            return csvData
                 .Skip(3) // Skip the header or any unwanted rows
                 .Take(5) // Take the next 5 rows (can be adjusted as needed)
                 .Select(row => row.Split(',')) // Split each row into columns
                 .Select(columns => new KeywordRanking
                 {
                     Keyword = columns[0], // First column is the keyword
                     Rank = columns[1], // Second column is the rank
                     CVR = columns[3], // Fourth column is the conversion rate (CVR)
                     Impressions = columns[4], // Fifth column is the number of impressions
                     Clicks = columns[5], // Sixth column is the number of clicks
                     Spend = columns[6], // Seventh column is the spend amount
                     TotalSales = columns[7] // Eighth column is the total sales
                 })
                 .OrderBy(row => row.Rank); // Order the results by rank
        }
    }
}
