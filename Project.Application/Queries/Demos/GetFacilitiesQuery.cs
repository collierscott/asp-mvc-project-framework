using Project.Application.Maps.Demos;
using Infrastructure.Data;

namespace Project.Application.Queries.Demos
{

    public class GetFacilitiesQuery : SqlQuery
    {

        public GetFacilitiesQuery()
        {

            Id = "GetFacilities";

            Query = @"";

            ObjectMap = new FacilityMap();

        }
         
    }

}