namespace GoogleSheetsAPI.Models
{
    public class SalesData
    {
        public DateTime Date { get; set; }
        public decimal DailySales { get; set; }
        public decimal MonthlyGoal { get; set; }
        public decimal GoalDifferencePercentage { get; set; } // Ny beräknad kolumn som visar skillnaden mellan försäljning och målet
    }
}
