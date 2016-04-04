using System.Linq;
using System.Web.Mvc;
using Project.Application.Services.Abstract;
using log4net;

namespace Project.WebUI.Controllers {

    public class NavController : BaseController
    {

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMenuService _service;

        public NavController(IMenuService serv)
        {
            _service = serv;
        }

        public PartialViewResult Menu(string selected = "", object parameters = null)
        {

            var items = _service.GetTopMenuItems(parameters).ToList();

            foreach (var item in items)
            {

                //If a URL had not already been set
                if (string.IsNullOrWhiteSpace(item.Url))
                {
                    // ReSharper disable once RedundantAnonymousTypePropertyName
                    item.Url = Url.Action(item.ActionName, item.ControllerName);
                }

                item.IsActive = selected.Equals(item.Text);


                foreach (var child in item.Children)
                {
                    if (string.IsNullOrWhiteSpace(child.Url))
                    {
                        // ReSharper disable once RedundantAnonymousTypePropertyName
                        child.Url = Url.Action(child.ActionName, child.ControllerName);
                    }

                }

            }

            return PartialView("Partial/Nav/_TopMenu", items);

        }

    }

}
