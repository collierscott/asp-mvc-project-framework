using System.Collections.Generic;
using System.Web.Mvc;

namespace Project.WebUI.Models.Demos.PageModels
{

    public class MultiselectPageModel
    {

        public string FacilityId { get; set; }
        public string WaferSizes { get; set; }
        public string RouteGroups { get; set; }
        public string RouteFamilies { get; set; }
        public string Series { get; set; }

        public IList<SelectListItem> WaferSizeItems { get; set; }
        public IList<SelectListItem> RouteGroupItems { get; set; }
        public IList<SelectListItem> RouteFamilyItems { get; set; }
        public IList<SelectListItem> SerieItems { get; set; }

        public MultiselectPageModel()
        {
            WaferSizeItems = new List<SelectListItem>();
            RouteGroupItems = new List<SelectListItem>();
            RouteFamilyItems = new List<SelectListItem>();
            SerieItems = new List<SelectListItem>();
        }
         
    }

}