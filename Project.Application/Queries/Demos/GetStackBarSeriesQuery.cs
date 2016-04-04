using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetStackBarSeriesQuery : SqlQuery
    {

        public GetStackBarSeriesQuery(string facility)
        {

            Id = "GetStackedBarSeriesQuery";

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
                {"SERIES_ID", "Id"},
                {"SERIES_NAME", "SeriesName"},
                {"SERIES_COLOR", "SeriesColor"},
                {"SORT_ORDER", "SeriesSortOrder"}

            };

        }

    }

}
