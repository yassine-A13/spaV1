using System.Collections.Generic;

namespace spaV1.Models
{
    public class StatisticsViewModel
    {
        public StatisticsViewModel()
        {
            TopServices = new List<TopServiceViewModel>();
        }

        public int TotalServices { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalClients { get; set; }
        public int TotalAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<TopServiceViewModel> TopServices { get; init; }
    }

    public class TopServiceViewModel
    {
        public string ServiceName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}