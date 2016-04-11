using System;
using Project.Application.Models.Charting;
using Project.Application.Queries.Demos;
using Project.Application.Repositories.Abstract;
using Project.Application.Services.Abstract;
using Project.Domain.Models.Entities;
using log4net;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Project.Application.Models;
using Project.Application.Models.Demos;
using Project.Application.Queries;
using Infrastructure.Data.Notify;
using Infrastructure.Data.Utilities;

namespace Project.Application.Services
{

    public class ProjectService : IProjectService
    {

        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private IProjectRepository _repository;
        private bool _disposed;

        public Notifications Messages { get; set; }

        /// <summary>
        /// Constructor that accepts a connection to the database
        /// </summary>
        /// <param name="repo"></param>
        public ProjectService(IProjectRepository repo)
        {

            _repository = repo;

            Messages = new Notifications();

            if (_repository.Messages != null)
            {
                Messages = _repository.Messages;
            }

        }

        /// <summary>
        /// Finds information about a Faciilty in the database
        /// </summary>
        /// <param name="value">The Facility Id or Name</param>
        /// <returns>Facility</returns>
        public Facility FindFacility(string value)
        {
            var facility = _repository.FindFacility(value);
            return facility;
        }

        /// <summary>
        /// Finds the Name of a Facility
        /// </summary>
        /// <param name="facility">Name or Id of facility to get</param>
        /// <returns>String which is Name of the Facility</returns>
        public string FindFacilityName(string facility)
        {
            var item = FindFacility(facility);
            var name = "";

            if (item != null)
            {
                name = item.Name;
            }

            return name;
        }

        public IEnumerable<Facility> GetAllFacilities()
        {
            return _repository.GetAllFacilities();
        } 

        #region Module Queries
        /// <summary>
        /// Finds the Name of a Module
        /// </summary>
        /// <param name="facility">FacilityId of Module</param>
        /// <param name="module">Module Name or Id</param>
        /// <returns>Name of Module</returns>
        public string FindModuleName(string facility, string module)
        {

            string name = "";

            var item = FindModule(facility, module);

            if (item != null)
            {
                name = item.Name;
            }

            return name;

        }

        /// <summary>
        /// Find a Module
        /// </summary>
        /// <param name="facility">FacilityId of Module</param>
        /// <param name="value">Module Id or Name</param>
        /// <returns></returns>
        public Module FindModule(string facility, string value)
        {
            var query = new FindModuleQuery(facility, value);
            var module = _repository.GetOneEntity<Module>(query);

            Messages.Add(query.Message);

            return module;

        }

        /// <summary>
        /// Builds Modules and all items that it owns (i.e. toolsets, tools, ...). The general idea is one query is used
        /// to obtain all of the information.
        /// </summary>
        /// <param name="facility"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public IEnumerable<Module> BuildModules(string facility, string m = null)
        {

            IList<Module> items = new List<Module>();
            var query = new BuildModulesQuery(facility, m);
            Messages.Add(query.Message);

            using (var reader = _repository.BuildModules(facility, m))
            {

                using (var data = new DataTable())
                {

                    if (reader != null)
                    {

                        data.Load(reader);

                        var modules = (from row in data.AsEnumerable()
                                       where facility.Equals(row.Field<string>("FACILITY"))
                                       select new Module
                                       {
                                           Id = row.Field<string>("MODULE"),
                                           Name = row.Field<string>("MODULE_DISPLAY"),
                                           FacilityId = row.Field<string>("FACILITY")
                                       }).Distinct(new PropertyComparator<Module>("Id")).OrderBy(o => o.Name, new AlphaNumComparator()).ToList();

                        foreach (var mod in modules)
                        {

                            var mod1 = mod;

                            var toolsets = (from row in data.AsEnumerable()
                                            where facility.Equals(row.Field<string>("FACILITY")) && mod1.Id.Equals(row.Field<string>("MODULE"))
                                            select new EquipmentFamily
                                            {
                                                Id = row.Field<string>("TOOLSET"),
                                                Name = row.Field<string>("TOOLSET_DISPLAY"),
                                                FacilityId = row.Field<string>("FACILITY"),
                                                ModuleId = row.Field<string>("MODULE")
                                            }).Distinct(new PropertyComparator<EquipmentFamily>("Id")).OrderBy(o => o.Name, new AlphaNumComparator()).ToList();


                            mod.EquipmentFamilies = toolsets;

                            foreach (var ts in toolsets)
                            {

                                var mod2 = mod;
                                var ts1 = ts;

                                var tools = (from row in data.AsEnumerable()
                                             where facility.Equals(row.Field<string>("FACILITY")) && mod2.Id.Equals(row.Field<string>("MODULE")) && ts1.Id.Equals(row.Field<string>("TOOLSET"))
                                             select new Tool
                                             {
                                                 Id = row.Field<string>("TOOL"),
                                                 Name = row.Field<string>("TOOL_DISPLAY"),
                                                 FacilityId = row.Field<string>("FACILITY"),
                                                 ModuleId = row.Field<string>("MODULE"),
                                                 EquipmentFamilyId = row.Field<string>("TOOLSET")
                                             }).Distinct(new PropertyComparator<Tool>("Id")).OrderBy(o => o.Name, new AlphaNumComparator()).ToList();

                                ts.Tools = tools;

                            }

                            items.Add(mod);
                        }

                    }

                }

            }

            return items;
        }
        #endregion

        public IEnumerable<ChartItem> GetStackedBarSeries(string facility)
        {
            var items = _repository.GetStackedBarSeries(facility).ToList();
            return items;
        }

        public IEnumerable<ChartItem> BuildStackedBarItems(string facility)
        {
            var items = new List<ChartItem>();

            var series = GetStackedBarSeries(facility).ToList();

            var results = _repository.GetStackedBarItems(facility);

            var categories = (from rows in results
                              group rows by new
                              {
                                  Label = rows.XLabel,
                                  SortOrder = rows.XLabelSortOrder
                              } into grp
                              select new
                              {
                                  grp.Key.Label,
                                  grp.Key.SortOrder
                              }
                ).OrderBy(o => o.SortOrder).ToList();

            //Need a chart item for every series and category (xlabel) combination to make sure series
            //display in the correct order and are availble to legend. There are several way to do this.
            //One is to do a cartesian join on xlabels and series in a SQL query and LEFT JOIN onto the
            //cartesian result. But, this can result in a large number of rows coming from the database. 
            // - Scott Collier 11/12/2014
            foreach (var s in series)
            {

                foreach (var category in categories)
                {

                    ChartItem ci;

                    var cItems = (from result in results
                              where result.SeriesName.Equals(s.SeriesName, StringComparison.OrdinalIgnoreCase)
                                && result.XLabel.Equals(category.Label, StringComparison.OrdinalIgnoreCase)
                              select new ChartItem
                              {
                                  Id = result.Id,
                                  SeriesColor = result.SeriesColor,
                                  SeriesName = result.SeriesName,
                                  SeriesSortOrder = result.SeriesSortOrder,
                                  XLabel = result.XLabel,
                                  XLabelSortOrder = result.XLabelSortOrder,
                                  YValue = result.YValue

                              }

                    ).ToList();

                    if (cItems.Any())
                    {
                        ci = cItems.First();
                    }
                    else
                    {

                        ci = new ChartItem
                        {
                            Id = s.Id,
                            SeriesColor = s.SeriesColor,
                            SeriesName = s.SeriesName,
                            SeriesSortOrder = s.SeriesSortOrder,
                            XLabel = category.Label,
                            XLabelSortOrder = category.SortOrder,
                            YValue = 0.0
                        };

                    }

                    items.Add(ci);

                }
                
            }

            return items;
        }

        public IEnumerable<ChartItem> GetDemosChartItems(string facilities, string waferSizes, string routeGroup, string routeFamily, string ctmSeries, bool isExcel = false)
        {
            var items = new List<ChartItem>();

            var f = BuildFilter(facilities, waferSizes, routeGroup, routeFamily, ctmSeries);
            var query = new GetDemosChartItemsQuery(f);

            Messages.Add(query.Message);

            using (var reader = _repository.GetDemosChartItems(f))
            {

                if (reader != null)
                {

                    var series = new List<SeriesItem>();

                    while (reader.Read())
                    {

                        var lotQty = DataParser.GetInt(reader["TOTAL_LOTS"].ToString());
                        var wfrQty = DataParser.GetInt(reader["TOTAL_WFR_QTY"].ToString());
                        var dieQty = DataParser.GetInt(reader["TOTAL_DIE_QTY"].ToString());

                        var wfrUnit = reader["TOTAL_WFR_QTY_DESC"].ToString();
                        var dieUnit = reader["TOTAL_DIE_QTY_DESC"].ToString();
                        var lotUnit = reader["TOTAL_LOTS_DESC"].ToString();

                        wfrUnit = !string.IsNullOrEmpty(wfrUnit) && wfrUnit.Length > 1 ? wfrUnit.Substring(0, 1).ToUpper() + wfrUnit.Substring(1) : wfrUnit;
                        dieUnit = !string.IsNullOrEmpty(dieUnit) && dieUnit.Length > 1 ? dieUnit.Substring(0, 1).ToUpper() + dieUnit.Substring(1) : dieUnit;

                        var si = new SeriesItem
                        {
                            Id = reader["SERIES"].ToString(),
                            Name = reader["SERIES_NAME"].ToString(),
                            Category = reader["XLABEL"].ToString(),
                            CategorySortOrder = DataParser.GetInt(reader["TIME_SORT_ORDER"].ToString()),
                            Value = DataParser.GetDouble(reader["YVALUE"].ToString()),
                            SeriesColor = reader["SERIES_COLOR"].ToString(),
                            SortOrder = DataParser.GetInt(reader["SERIES_SORT_ORDER"].ToString()),
                            NumLots = lotQty,
                            NumWafers = wfrQty,
                            NumDie = dieQty,
                            WaferUnit = wfrUnit,
                            DieUnit = dieUnit,
                            LotUnit = lotUnit
                        };

                        series.Add(si);

                    }

                    //Hover message data
                    var categories = (
                        from c in series
                        select c.Category
                        ).Distinct().ToList();

                    var dieTotals = new Dictionary<string, long>();
                    var wfrTotals = new Dictionary<string, int>();
                    var lotTotals = new Dictionary<string, int>();

                    foreach (var cat in categories)
                    {
                        var dt = (from v in series
                                  where v.Category.Equals(cat)
                                  select (long)v.NumDie
                            ).Sum();

                        var wt = (from v in series
                                  where v.Category.Equals(cat)
                                  select v.NumWafers
                            ).Sum();

                        var lt = (from v in series
                                  where v.Category.Equals(cat)
                                  select v.NumLots
                            ).Sum();

                        dieTotals.Add(cat, dt);
                        wfrTotals.Add(cat, wt);
                        lotTotals.Add(cat, lt);

                    }

                    //Add hover message data to series
                    foreach (var s in series)
                    {

                        var message = new DemoChartHoverMessage
                        {
                            Id = s.Id,
                            TimePeriod = s.Category,
                            WaferUnits = s.WaferUnit,
                            DieUnits = s.DieUnit,
                            TotalWafers = wfrTotals[s.Category].ToString("N0"),
                            NumDies = s.NumDie.ToString("N0"),
                            NumWafers = s.NumWafers.ToString("N0"),
                            NumLots = s.NumLots.ToString("N0"),
                            TotalDie = dieTotals[s.Category].ToString("N0"),
                            TotalLots = lotTotals[s.Category].ToString("N0"),
                            FacilityId = s.Id,
                            IntLots = s.NumLots
                        };

                        var item = new ChartItem
                        {
                            Id = s.Id,
                            SeriesName = s.Name,
                            XLabel = s.Category,
                            XLabelSortOrder = s.CategorySortOrder,
                            YValue = s.Value,
                            SeriesColor = s.SeriesColor,
                            SeriesSortOrder = s.SortOrder,
                            HoverMessage = message
                        };

                        items.Add(item);

                    }

                }

            }

            return items;
        }

        public IEnumerable<DropdownItem> GetWaferSizeDropdownItems(string facility)
        {
            return _repository.GetWaferSizeDropdownItems(facility);
        }

        public IEnumerable<DropdownItem> GetRouteGroupDropdownItems(string facility, string waferSize)
        {
            return _repository.GetRouteGroupDropdownItems(facility, waferSize);
        }

        public IEnumerable<DropdownItem> GetRouteFamilyDropdownItems(string facility, string waferSize, string routeGroup)
        {
            return _repository.GetRouteFamilyDropdownItems(facility, waferSize, routeGroup);
        }

        public IEnumerable<DropdownItem> GetSeriesDropdownItems(string facility, string waferSize, string routeGroup, string routeFamily)
        {
            return _repository.GetSeriesDropdownItems(facility, waferSize, routeGroup, routeFamily);
        }

        private string BuildFilter(string facility, string waferSizes, string routeGroups, string routeFamilies, string ctmSeries, bool useSeries = false)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(facility) && !facility.Equals("ALL", StringComparison.OrdinalIgnoreCase) && !useSeries)
            {
                sb.Append(" AND facility = '" + facility + "'");
            }

            if (!string.IsNullOrEmpty(facility) && !facility.Equals("ALL", StringComparison.OrdinalIgnoreCase) && useSeries)
            {
                sb.Append(" AND series = '" + facility + "'");
            }

            if (!string.IsNullOrEmpty(waferSizes) && waferSizes.Length > 0 && !waferSizes.Equals("NONE", StringComparison.OrdinalIgnoreCase) && !waferSizes.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {

                sb.Append(" AND wafer_size IN (" + QueryHelper.AddSingleQuotes(waferSizes) + ")");
            }

            if (!string.IsNullOrEmpty(routeGroups) && routeGroups.Length > 0 && !routeGroups.Equals("NONE", StringComparison.OrdinalIgnoreCase) && !routeGroups.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(" AND route_group IN (" + QueryHelper.AddSingleQuotes(routeGroups) + ")");
            }

            if (!string.IsNullOrEmpty(routeFamilies) && routeFamilies.Length > 0 && !routeFamilies.Equals("NONE", StringComparison.OrdinalIgnoreCase) && !routeFamilies.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(" AND route_family IN (" + QueryHelper.AddSingleQuotes(routeFamilies) + ")");
            }

            if (!string.IsNullOrEmpty(ctmSeries) && ctmSeries.Length > 0 && !ctmSeries.Equals("NONE", StringComparison.OrdinalIgnoreCase) && !ctmSeries.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                sb.Append(" AND ctm_series IN (" + QueryHelper.AddSingleQuotes(ctmSeries) + ")");
            }

            return sb.ToString();
        }

        #region Implementation of Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes resources used.
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {

            if (!_disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }
            }

            _disposed = true;
        }
        #endregion

    }

}
