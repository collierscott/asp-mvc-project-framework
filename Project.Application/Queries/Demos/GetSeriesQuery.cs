using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetSeriesQuery : SqlQuery
    {

        public GetSeriesQuery(string facility, string waferSize, string routeGroup, string routeFamily)
        {

            Id = "GetSeriesQuery";

            var hasWaferSize = !string.IsNullOrWhiteSpace(waferSize);
            var hasRouteGroup = !string.IsNullOrWhiteSpace(routeGroup);
            var hasRouteFamily = !string.IsNullOrWhiteSpace(routeFamily);

            Query = string.Format(@"");

            Parameters = new List<QueryParameter>
            {
                new QueryParameter
                {
                    ParameterName = "FACILITY_ID",
                    Value = facility
                }
            };

            ObjectMap = new ObjectMapper
            {
                {"CTM_SERIES", "Id"},
                {"CTM_SERIES_DISPLAY", "Name"},
                {"SORT_ORDER", "SortOrder"}
            };

        }
         
    }

}