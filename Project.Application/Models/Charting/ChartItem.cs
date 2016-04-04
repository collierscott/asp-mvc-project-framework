using Project.Domain.Models.Entities;

namespace Project.Application.Models.Charting
{

    public class ChartItem : Entity<string>
    {

        public string FacilityId { get; set; }
        public string SeriesName { get; set; }
        public string XLabel { get; set; }
        public double XValue { get; set; }
        public double YValue { get; set; }
        public int XLabelSortOrder { get; set; }
        public int SeriesBorderWidth { get; set; }
        public string SeriesChartType { get; set; }
        public string SeriesColor { get; set; }
        public int SeriesMarkerSize { get; set; }
        public string SeriesMarkerStyle { get; set; }
        public int SeriesSortOrder { get; set; }
        public string SeriesXValueType { get; set; }
        public string SeriesYAxisType { get; set; }
        public string SeriesCustomProperties { get; set; }
        public string ValueMessage { get; set; }
        public string ValueColor { get; set; }
        public ChartHoverMessage HoverMessage { get; set; }

        public ChartItem()
        {
            HoverMessage = new ChartHoverMessage();
        }

    }

}
