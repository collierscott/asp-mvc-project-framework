using System.Collections.Generic;
using Infrastructure.Data;

namespace Project.Application.Queries
{

    public class BuildModulesQuery : SqlQuery
    {

        public BuildModulesQuery(string facility, string value)
        {

            Id = "Build Modules Query";

            Query = string.Format(@""
           );

            Parameters= new List<QueryParameter>
            {
                new QueryParameter { ParameterName = "FACILITY_ID", Value = facility }
            };

            if (!string.IsNullOrWhiteSpace(value))
            {
                Parameters.Add(new QueryParameter { ParameterName = "MODULE_ID", Value = value });
            }

        }

    }

}
