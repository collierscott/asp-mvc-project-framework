using System.Web.Optimization;

namespace Project.WebUI
{
    
    public class BundleConfig
    {

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            //Sdould set to true when publishing to production if usng bundles
            BundleTable.EnableOptimizations = false;

        }

    }

}