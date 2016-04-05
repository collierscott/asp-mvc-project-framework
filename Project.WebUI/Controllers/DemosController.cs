using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Project.Application.Services.Abstract;
using Project.WebUI.Filters;
using Project.WebUI.Models.Demos.PageModels;
using Project.WebUI.Utilities;
using Infrastructure.Data.Utilities;
using log4net;

namespace Project.WebUI.Controllers {

    public class DemosController : BaseController
    {

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IProjectService _service;

        public DemosController(IProjectService service)
        {

            _service = service;

            if (_service.Messages != null)
            {
                Messages = _service.Messages;
            }

        }

        [FacilityRequiredFilter]
        public ActionResult Index()
        {
            ViewBag.Page = "Demos";
            ViewBag.Messages = Messages;

            return View();

        }

        public PartialViewResult FacilityDropdown(string facility)
        {
            
            facility = Utility.GetFacility(facility);

            var facilities = _service.GetAllFacilities();

            var selects = (from f in facilities
                           orderby f.SortOrder
                select new SelectListItem
                {
                    Value = f.Id,
                    Text = f.Name,
                    Selected = f.Id.Equals(facility)
                }
            ).ToList();

            return PartialView("Partial/Parts/_Dropdown", selects);

        }

        /// <summary>
        /// Create a Chart
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        [FacilityRequiredFilter]
        public ActionResult CreateChart(string facility = "")
        {

            ViewBag.Facility = facility;

            var item = _service.FindFacility(facility);

            var page = new ChartPageModel
            {
                Facility = item
            };

            var modules = _service.BuildModules(facility).OrderBy(o => o.Name, new AlphaNumComparator());

            item.Modules = modules.ToList();

            ViewBag.Messages = Messages;

            return View(page);

        }

        /// <summary>
        /// Create DataTable
        /// </summary>
        /// <param name="facility">facility name</param>
        /// <returns></returns>
        [FacilityRequiredFilter]
        public ActionResult DataTableGrid(string facility)
        {

            ViewBag.Facility = facility;

            var item = _service.FindFacility(facility);

            var page = new GridPageModel
            {
                Facility = item
            };

            var modules = _service.BuildModules(facility).OrderBy(o => o.Name, new AlphaNumComparator());

            item.Modules = modules.ToList();
            ViewBag.Messages = Messages;

            return View(page);

        }

        [FacilityRequiredFilter]
        public ActionResult CreateStackedbarChart(string facility)
        {

            var page = new StackedChartPageModel
            {
                ChartItems = _service.BuildStackedBarItems(facility).ToList()
            };

            return View(page);

        }

        [FacilityRequiredFilter]
        public ActionResult CreateMultiselects(string facility, string waferSize, string routeGroup, string routeFamily, string series)
        {

            var waferSizes = _service.GetWaferSizeDropdownItems(facility);
            var routeGroups = _service.GetRouteGroupDropdownItems(facility, waferSize);
            var routeFamilies = _service.GetRouteFamilyDropdownItems(facility, waferSize, routeGroup);
            var seriesItems = _service.GetSeriesDropdownItems(facility, waferSize, routeGroup, routeFamily);
            var results = _service.GetDemosChartItems(facility, waferSize, routeGroup, routeFamily, series);

            var page = new MultiselectPageModel
            {
                FacilityId = facility,
                WaferSizes = waferSize,
                RouteGroups = routeGroup,
                RouteFamilies = routeFamily,
                Series = series,
                WaferSizeItems = (
                    from ws in waferSizes
                    select new SelectListItem
                    {
                        Text = ws.Name,
                        Value = ws.Id,
                        Selected = IsSelected(ws.Id, waferSize)
                    }
                ).ToList(),
                RouteGroupItems = (
                    from rg in routeGroups
                    select new SelectListItem
                    {
                        Text = rg.Name,
                        Value = rg.Id,
                        Selected = IsSelected(rg.Id, routeGroup)
                    }
                ).ToList(),
                RouteFamilyItems = (
                    from rf in routeFamilies
                    select new SelectListItem
                    {
                        Text = rf.Name,
                        Value = rf.Id,
                        Selected = IsSelected(rf.Id, routeFamily)
                    }
                
                ).ToList(),
                SerieItems = (
                    from s in seriesItems
                    select new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id,
                        Selected = IsSelected(s.Id, series)
                    }
                ).ToList()
            };

            return View(page);

        }

        [OutputCache(CacheProfile = "5MinuteCacheProfile")]
        public JsonResult GetWaferSizes(string facility, string waferSize = "")
        {

            var items = new List<SelectListItem>();

            if (string.IsNullOrWhiteSpace(facility) || facility.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                facility = string.Empty;
            }

            var results = _service.GetWaferSizeDropdownItems(facility).OrderBy(o => o.SortOrder).ToList();

            foreach (var item in results)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.Name,
                        Selected = string.IsNullOrWhiteSpace(waferSize) || waferSize.Equals(item.Id, StringComparison.OrdinalIgnoreCase) || IsSelected(item.Id, waferSize)
                            || waferSize.Equals("ALL", StringComparison.OrdinalIgnoreCase)
                    });
            }

            return Json(new { selects = items, count = items.Count });

        }

        [OutputCache(CacheProfile = "5MinuteCacheProfile")]
        public JsonResult GetRouteGroups(string facility, string waferSize = "", string routeGroup = "")
        {

            var items = new List<SelectListItem>();

            if (string.IsNullOrWhiteSpace(facility) || facility.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                facility = string.Empty;
            }

            var results = _service.GetRouteGroupDropdownItems(facility, waferSize).OrderBy(o => o.SortOrder).ToList();

            foreach (var item in results)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.Name,
                        Selected = string.IsNullOrWhiteSpace(routeGroup) || routeGroup.Equals(item.Id, StringComparison.OrdinalIgnoreCase) || IsSelected(item.Id, routeGroup)
                            || routeGroup.Equals("ALL", StringComparison.OrdinalIgnoreCase)
                    });
            }

            return Json(new { selects = items, count = items.Count });

        }

        [OutputCache(CacheProfile = "5MinuteCacheProfile")]
        public JsonResult GetRouteFamilies(string facility, string waferSize = "", string routeGroup = "", string routeFamily = "")
        {

            var items = new List<SelectListItem>();

            if (string.IsNullOrWhiteSpace(facility) || facility.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                facility = string.Empty;
            }

            var results = _service.GetRouteFamilyDropdownItems(facility, waferSize, routeGroup).OrderBy(o => o.SortOrder).ToList();

            foreach (var item in results)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.Name,
                        Selected = string.IsNullOrWhiteSpace(routeFamily) || routeFamily.Equals(item.Id, StringComparison.OrdinalIgnoreCase) || IsSelected(item.Id, routeFamily)
                            || routeFamily.Equals("ALL", StringComparison.OrdinalIgnoreCase)
                    });
            }

            return Json(new { selects = items, count = items.Count });

        }

        [OutputCache(CacheProfile = "5MinuteCacheProfile")]
        public JsonResult GetSeries(string facility, string waferSize = "", string routeGroup = "", string routeFamily = "", string series = "")
        {

            var items = new List<SelectListItem>();

            if (string.IsNullOrWhiteSpace(facility) || facility.Equals("ALL", StringComparison.OrdinalIgnoreCase))
            {
                facility = string.Empty;
            }

            var results = _service.GetSeriesDropdownItems(facility, waferSize, routeGroup, routeFamily).OrderBy(o => o.SortOrder).ToList();

            foreach (var item in results)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = item.Id,
                        Text = item.Name,
                        Selected = string.IsNullOrWhiteSpace(series) || series.Equals(item.Id, StringComparison.OrdinalIgnoreCase) || IsSelected(item.Id, series)
                            || series.Equals("ALL", StringComparison.OrdinalIgnoreCase)
                    });
            }

            return Json(new { selects = items, count = items.Count });

        }

        /// <summary>
        /// Should item be selected
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool IsSelected(string value, string values)
        {

            if (string.IsNullOrWhiteSpace(values)) return false;

            string[] tokens = values.Split(',');

            for (int x = 0; x < tokens.Length; x++)
            {
                if (tokens[x].Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

            }

            return false;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    _service.Dispose();
        //    base.Dispose(disposing);
        //}

    }

}
