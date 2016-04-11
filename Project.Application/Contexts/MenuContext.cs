using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using Infrastructure.Data;
using Infrastructure.Data.Notify;
using log4net;

namespace Project.Application.Contexts
{

    public class MenuContext : DatabaseContext
    {

        // ReSharper disable once InconsistentNaming
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // ReSharper disable once NotAccessedField.Local
        private bool _disposed;
        // ReSharper disable once NotAccessedField.Local
        private readonly DbProviderFactory _provider;
        // ReSharper disable once NotAccessedField.Local
        private readonly string _connectionString;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly string _name;

        public ObjectContext BuildObjectContext()
        {
            throw new NotImplementedException();
        }

        public DbConnection Connection { get { return _connection; } }

        public IDbDataAdapter Adapter
        {
            get { return _provider.CreateDataAdapter(); }
        }

        // ReSharper disable once UnassignedField.Compiler
        private DbConnection _connection;

        public Notifications Messages { get; set; }

        public MenuContext()
        {
            //Mainly for testing
        }

        public MenuContext(string connectionName)
        {

            if (Messages == null)
            {
                Messages = new Notifications();
            }

            if (string.IsNullOrWhiteSpace(connectionName))
            {
                _log.Error("connectionName is Null");
                //throw new ArgumentNullException("connectionName");
            }
            else
            {

                var connString = ConfigurationManager.ConnectionStrings[connectionName];

                if (connString == null)
                {
                    _log.Error(string.Format("Failed to find connection string named '{0}' in web.config.", connectionName));
                    //throw new ConfigurationErrorsException(string.Format("Failed to find connection string named '{0}' in web.config.", connectionName));
                }

                if (connString != null)
                {
                    _name = connString.ProviderName;
                    _provider = DbProviderFactories.GetFactory(_name);
                    _connectionString = connString.ConnectionString;
                }
            }

        }

        public void Close()
        {

            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }

                _connection.Dispose();

            }

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

            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _log.Debug("Closing Context Connection");
                Close();
            }

            _disposed = true;
        }
        #endregion

    }

}