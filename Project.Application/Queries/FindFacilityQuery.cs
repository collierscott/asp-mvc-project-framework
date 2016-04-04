using System.Collections.Generic;
using Project.Application.Maps.Demos;
using Infrastructure.Data;

namespace Project.Application.Queries
{

    /// <summary>
    /// Query to get a specific facility
    /// </summary>
    public class FindFacilityQuery : SqlQuery
    {

        /// <summary>
        /// Query to get a specific facility
        /// </summary>
        /// <param name="value">Facility to get</param>
        public FindFacilityQuery(string value)
        {
            Id = "GetFacilityQuery";

            Query = @"";

            Parameters = new List<QueryParameter>
            {
                new QueryParameter { ParameterName = "FACILITY_ID", Value = value },
                new QueryParameter { ParameterName = "FACILITY_DISPLAY", Value = value }
            };

            ObjectMap =new FacilityMap();

        }

    }

}
