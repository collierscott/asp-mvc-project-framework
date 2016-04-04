using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetDemosChartItemsQuery : SqlQuery
    {

        public GetDemosChartItemsQuery(string filter, bool isExcel = false)
        {

            Id = "GetDemosChartItemsQuery";

            Query = string.Format(@"",
                 filter.Length > 0 ? filter : ""
                );

        }
         
    }

}