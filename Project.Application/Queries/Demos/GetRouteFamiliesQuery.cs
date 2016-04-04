using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetRouteFamiliesQuery : SqlQuery
    {

        public GetRouteFamiliesQuery(string facility, string waferSize, string routeGroup)
        {

            Id = "GetRouteFamiliesQuery";

            var hasWaferSize = !string.IsNullOrWhiteSpace(waferSize);
            var hasRouteGroup = !string.IsNullOrWhiteSpace(routeGroup);

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
                {"ROUTE_FAMILY", "Id"},
                {"ROUTE_FAMILY_DISPLAY", "Name"},
                {"SORT_ORDER", "SortOrder"}
            };

        }
         
    }

}