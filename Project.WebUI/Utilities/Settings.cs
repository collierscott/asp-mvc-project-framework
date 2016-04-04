using System.Configuration;
using System.Data.Common;

namespace Project.WebUI.Utilities {
    
    public static class Settings {

        public static string DatabaseConnectionName
        {
            get
            {
                return ConfigurationManager.AppSettings["DBConnectionStringName"];
            }
        }

        public static string DefaultController
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DefaultController");
            }
        }

        public static string CulturalInfo
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DefaultCulturalInfo") ?? "en-US";
            }
        }

        public static string DefaultFacility
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DefaultFacility");
            }
        }

        public static string Application
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("Application");
            }
        }

        public static ConnectionStringSettings ConnectionStringSettings
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[DatabaseConnectionName];
            }
        }

        public static DbProviderFactory DbProviderFactory
        {
            get
            {
                return DbProviderFactories.GetFactory(ConnectionStringSettings.ProviderName);
            }
        }

        public static string ClientName
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("ClientName");
            }
        }
    }
}