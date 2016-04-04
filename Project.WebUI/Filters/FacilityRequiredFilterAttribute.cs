using System.Web.Mvc;
using System.Web.Routing;
using Project.WebUI.Utilities;
using log4net;

namespace Project.WebUI.Filters
{

    /// <summary>
    /// Filter used when a facility name is required. If this filter is used, it will check to see if a filter parameter
    /// is found and if it has a value. If not, the DefaultFacility listed in web.config is used and context
    /// is redirected.
    /// </summary>
    public class FacilityRequiredFilterAttribute : ActionFilterAttribute
    {

        // ReSharper disable once InconsistentNaming
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly string _parameter;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //var filter = filterContext.ActionDescriptor.GetCustomAttributes(typeof (string), false).Cast<string>();

            if (!filterContext.ActionParameters.ContainsKey("facility") ||
                filterContext.ActionParameters["facility"] == null || string.IsNullOrEmpty(filterContext.ActionParameters["facility"].ToString()))
            {
                var facility = Settings.DefaultFacility;
                var action = filterContext.ActionDescriptor.ActionName;
                var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                var rvd = new RouteValueDictionary
                {
                    {"controller", controller},
                    {"action", action},
                    {"facility", facility}
                };

                filterContext.Result = new RedirectToRouteResult(rvd);

            }

            base.OnActionExecuting(filterContext);

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                _log.Debug("(Logging Filter) Exception thrown");
            }

            base.OnActionExecuted(filterContext);
        }

    }

}