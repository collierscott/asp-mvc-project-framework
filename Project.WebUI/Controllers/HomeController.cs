using System;
using System.Web.Mvc;
using Project.Application.Services.Abstract;
using Project.WebUI.Filters;
using Project.WebUI.Utilities;
using log4net;

namespace Project.WebUI.Controllers {

    public class HomeController : BaseController
    {

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [FacilityRequiredFilter]
        public ActionResult Index(string facility)
        {

            var fac = Utility.GetFacility(facility);

            ViewBag.Page = "Home";
            ViewBag.Messages = Messages;
            ViewBag.Facility = fac;

            return View();

        }

        public PartialViewResult LastUpdated()
        {
            ViewBag.Time = DateTime.Now.ToString("MMM dd h:mm tt");
            return PartialView("Partial/_LastUpdated");
        }

    }

}
