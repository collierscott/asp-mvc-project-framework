using Project.Application.Models.Charting;

namespace Project.Application.Models.Demos
{

    public class DemoChartHoverMessage : ChartHoverMessage
    {
        public string Url { get; set; }
        public string FacilityId { get; set; }
        public string TimePeriod { get; set; }
        public string WaferUnits { get; set; }
        public string NumWafers { get; set; }
        public string DieUnits { get; set; }
        public string NumDies { get; set; }
        public string NumLots { get; set; }
        public string TotalWafers { get; set; }
        public string TotalDie { get; set; }
        public string TotalLots { get; set; }
        public int IntLots { get; set; }
    }

}