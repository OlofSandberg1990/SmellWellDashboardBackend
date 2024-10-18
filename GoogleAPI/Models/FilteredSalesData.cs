namespace GoogleSheetsAPI.Models
{
    public class FilteredSalesData
    {
        public DateTime Date { get; set; }
        public decimal Sales { get; set; }
        public decimal Goal { get; set; }
        public decimal GoalDifference { get; set; }
    }

}
