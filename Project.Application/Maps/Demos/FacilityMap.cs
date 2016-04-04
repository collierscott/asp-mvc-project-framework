using Infrastructure.Data;

namespace Project.Application.Maps.Demos
{

    public class FacilityMap : ObjectMapper
    {

        public FacilityMap()
        {   
            Add("FACILITY", "Id");
            Add("FACILITY_DISPLAY", "Name");
            Add("FACILITY_SORT_ORDER", "SortOrder");
        }
         
    }

}