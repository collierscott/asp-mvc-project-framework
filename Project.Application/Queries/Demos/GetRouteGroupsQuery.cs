using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetRouteGroupsQuery : SqlQuery
    {

        public GetRouteGroupsQuery(string facility, string waferSize)
        {

            Id = "GetRouteGroupsQuery";

            var hasWaferSize = !string.IsNullOrWhiteSpace(waferSize);

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
                {"ROUTE_GROUP", "Id"},
                {"ROUTE_GROUP_DISPLAY", "Name"},
                {"SORT_ORDER", "SortOrder"}
            };
            
        }
         
    }

}