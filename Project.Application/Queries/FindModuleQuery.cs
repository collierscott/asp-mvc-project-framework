using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries
{

    /// <summary>
    /// A query and mapping to get a module
    /// </summary>
    public class FindModuleQuery : SqlQuery
    {

        /// <summary>
        /// A query to get and module
        /// </summary>
        /// <param name="facility">Facility of module</param>
        /// <param name="value">Module to get</param>
        public FindModuleQuery(string facility, string value)
        {

            Id = "GetModuleQuery";

            Query = @"";

            Parameters = new List<QueryParameter>
            {

                new QueryParameter { ParameterName = "FACILITY_ID", Value = facility },
                new QueryParameter { ParameterName = "MODULE_ID", Value = value },
                new QueryParameter { ParameterName = "MODULE_DISPLAY", Value = value }
            };

            ObjectMap = new ObjectMapper
            {
                {"FACILITY", "FacilityId"}, 
                {"MODULE", "Id"},
                {"MODULE_DISPLAY", "Name"}
            };

        }

    }

}
