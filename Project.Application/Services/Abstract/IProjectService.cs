using System;
using System.Collections.Generic;
using Project.Application.Models;
using Project.Application.Models.Charting;
using Project.Domain.Models.Entities;
using Infrastructure.Data.Notify;

namespace Project.Application.Services.Abstract
{

    public interface IProjectService : IDisposable
    {

        Notifications Messages { get; set; }

        /// NOTES 
        /// 'Get' methods get one or more basic entities from the database. 
        /// 'Build' methods build an object and may include other objects that it owns. Example:  BuildTools would create a tool and get all of its chambers, ports, and lots if needed
        /// 'Find' methods gets only one entity form the database
        string FindFacilityName(string facility);
        string FindModuleName(string facility, string module);

        Facility FindFacility(string facility);
        IEnumerable<Facility> GetAllFacilities();

        Module FindModule(string facility, string module);
        IEnumerable<Module> BuildModules(string facility, string module = null);

        IEnumerable<ChartItem> GetStackedBarSeries(string facility);
        IEnumerable<ChartItem> BuildStackedBarItems(string facility);

        IEnumerable<ChartItem> GetDemosChartItems(string facilities, string waferSizes, string routeGroup,
            string routeFamily, string ctmSeries, bool isExcel = false);

        #region dropdowns

        IEnumerable<DropdownItem> GetWaferSizeDropdownItems(string facility);
        IEnumerable<DropdownItem> GetRouteGroupDropdownItems(string facility, string waferSize);
        IEnumerable<DropdownItem> GetRouteFamilyDropdownItems(string facility, string waferSize, string routeGroup);
        IEnumerable<DropdownItem> GetSeriesDropdownItems(string facility, string waferSize, string routeGroup, string routeFamily);

        #endregion

    }

}
