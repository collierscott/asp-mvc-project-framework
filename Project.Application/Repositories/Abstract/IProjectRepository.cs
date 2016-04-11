using System.Collections.Generic;
using System.Data;
using Infrastructure.Data.Abstract;
using Infrastructure.Data.Notify;
using Project.Application.Models;
using Project.Application.Models.Charting;
using Project.Domain.Models.Entities;

namespace Project.Application.Repositories.Abstract
{
    public interface IProjectRepository : IRepository
    {
        Facility FindFacility(string value);
        IEnumerable<Facility> GetAllFacilities();
        Module FindModule(string facility, string value);
        IDataReader BuildModules(string facility, string m = null);
        IEnumerable<ChartItem> GetStackedBarSeries(string facility);
        IEnumerable<ChartItem> GetStackedBarItems(string facility);
        IDataReader GetDemosChartItems(string filter);
        IEnumerable<DropdownItem> GetWaferSizeDropdownItems(string facility);
        IEnumerable<DropdownItem> GetRouteGroupDropdownItems(string facility, string waferSize);
        IEnumerable<DropdownItem> GetRouteFamilyDropdownItems(string facility, string waferSize, string routeGroup);
        IEnumerable<DropdownItem> GetSeriesDropdownItems(string facility, string waferSize, string routeGroup,
            string routeFamily);
    }
}
