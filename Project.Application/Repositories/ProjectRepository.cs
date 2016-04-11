using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Project.Application.Repositories.Abstract;
using Infrastructure.Data;
using Infrastructure.Data.Utilities;
using log4net;
using Project.Application.Models;
using Project.Application.Models.Charting;
using Project.Application.Queries;
using Project.Application.Queries.Demos;
using Project.Application.Utilities;
using Project.Domain.Models.Entities;

namespace Project.Application.Repositories
{

    public class ProjectRepository : GenericRepository, IProjectRepository
    {

        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ProjectRepository(string context)
            : base(context)
        {}

        public Facility FindFacility(string value)
        {
            var query = new FindFacilityQuery(value);
            var facility = GetOneEntity<Facility>(query);
            Messages.Add(query.Message);
            return facility;
        }
        public IDataReader BuildModules(string facility, string m = null)
        {
            var query = new BuildModulesQuery(facility, m);
            Messages.Add(query.Message);
            return GetDataReader(query);
        }
        public IEnumerable<Facility> GetAllFacilities()
        {
            var query = new GetFacilitiesQuery();
            return GetAll<Facility>(query);
        }   
        public Module FindModule(string facility, string value)
        {
            var query = new FindModuleQuery(facility, value);
            var module = GetOneEntity<Module>(query);

            Messages.Add(query.Message);

            return module;
        }
        public IEnumerable<ChartItem> GetStackedBarSeries(string facility)
        {
            var query = new GetStackBarSeriesQuery(facility);
            return GetAll<ChartItem>(query).ToList();
        }
        public IEnumerable<ChartItem> GetStackedBarItems(string facility)
        {
            var query = new GetStackedBarItemsQuery(facility);
            return GetAll<ChartItem>(query).ToList();
        }
        public IDataReader GetDemosChartItems(string filter)
        {
            var query = new GetDemosChartItemsQuery(filter);

            Messages.Add(query.Message);

            return GetDataReader(query);
        }
        public IEnumerable<DropdownItem> GetWaferSizeDropdownItems(string facility)
        {
            var query = new GetWaferSizesQuery(facility);
            return GetAll<DropdownItem>(query);
        }
        public IEnumerable<DropdownItem> GetRouteGroupDropdownItems(string facility, string waferSize)
        {
            if (ItemHasNoValue.Check(waferSize))
            {
                waferSize = String.Empty;
            }
            else
            {
                waferSize = QueryHelper.AddSingleQuotes(waferSize);
            }

            var query = new GetRouteGroupsQuery(facility, waferSize);

            return GetAll<DropdownItem>(query);
        }
        public IEnumerable<DropdownItem> GetRouteFamilyDropdownItems(string facility, string waferSize, string routeGroup)
        {
            if (ItemHasNoValue.Check(waferSize))
            {
                waferSize = String.Empty;
            }
            else
            {
                waferSize = QueryHelper.AddSingleQuotes(waferSize);
            }

            if (ItemHasNoValue.Check(routeGroup))
            {
                routeGroup = String.Empty;
            }
            else
            {
                routeGroup = QueryHelper.AddSingleQuotes(routeGroup);
            }

            var query = new GetRouteFamiliesQuery(facility, waferSize, routeGroup);

            return GetAll<DropdownItem>(query);
        }
        public IEnumerable<DropdownItem> GetSeriesDropdownItems(string facility, string waferSize, string routeGroup, string routeFamily)
        {
            waferSize = ItemHasNoValue.Check(waferSize) ? String.Empty : QueryHelper.AddSingleQuotes(waferSize);
            routeGroup = ItemHasNoValue.Check(routeGroup) ? String.Empty : QueryHelper.AddSingleQuotes(routeGroup);
            routeFamily = ItemHasNoValue.Check(routeFamily) ? String.Empty : QueryHelper.AddSingleQuotes(routeFamily);

            var query = new GetSeriesQuery(facility, waferSize, routeGroup, routeFamily);

            return GetAll<DropdownItem>(query);
        }
    }
}
