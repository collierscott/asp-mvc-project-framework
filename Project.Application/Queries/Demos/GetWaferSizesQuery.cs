using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetWaferSizesQuery : SqlQuery
    {

        public GetWaferSizesQuery(string facility)
        {

            Id = "GetWaferSizesQuery";

            Query = @"";

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
                {"WAFER_SIZE", "Id"},
                {"WAFER_SIZE_DISPLAY", "Name"},
                {"SORT_ORDER", "SortOrder"}
            };

        }
         
    }

}