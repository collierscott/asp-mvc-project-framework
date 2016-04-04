using System.Web.Mvc;
using Infrastructure.Data.Notify;
using log4net;

namespace Project.WebUI.Controllers
{

    public class BaseController : Controller
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Notifications Messages { get; set; }

        public BaseController()
        {

            if (Messages == null)
            {
                Messages = new Notifications();
            }

        }

    }

}
