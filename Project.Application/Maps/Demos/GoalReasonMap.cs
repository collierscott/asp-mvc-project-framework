using Infrastructure.Data;

namespace Project.Application.Maps.Demos
{
    
    public class GoalReasonMap : ObjectMapper
    {

        public GoalReasonMap()
        {
            
            Add("PROCESS_FAMILY", "Id");
            Add("PROCESS_DISPLAY", "Xlabel");
            Add("PROCESS_SORT", "XLabelSortOrder ");
            Add("GOAL_REASON_SORT", "SeriesSortOrder");
            Add("GOAL_REASON_DISPLAY", "SeriesName");
            Add("GOAL_REASON_HTML_COLOR", "SeriesColor");
            Add("GOAL_BEGIN", "Yvalue");

        }

    }

}
