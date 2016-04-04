using System.Collections.Generic;
using Project.Application.Maps.Demos;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{
    
    public class GetStackedBarItemsQuery : SqlQuery
    {

        public GetStackedBarItemsQuery(string facility)
        {

            Id = "GetStackedBarItemsQuery";

            Query = @"";

            Parameters = new List<QueryParameter>
            {
                
                new QueryParameter
                {
                    ParameterName = "FACILITY_ID",
                    Value = facility
                }

            };

            ObjectMap = new GoalReasonMap();

        }

    }

}
