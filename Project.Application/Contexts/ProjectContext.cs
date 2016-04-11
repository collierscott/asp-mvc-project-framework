using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using Infrastructure.Data.Abstract;
using Infrastructure.Data.Notify;
using log4net;

namespace Project.Application.Contexts
{

    public class ProjectContext : IDatabaseContext
    {

        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        // ReSharper disable once NotAccessedField.Local
        private bool _disposed;
        // ReSharper disable once NotAccessedField.Local
        private readonly DbProviderFactory _provider;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly string _name;

        // ReSharper disable once NotAccessedField.Local
        private string _connectionString;

        // ReSharper disable once UnassignedField.Compiler
        private DbConnection _connection;

        public Notifications Messages { get; set; }

        public ObjectContext BuildObjectContext()
        {
            throw new NotImplementedException();
        }

        public DbConnection Connection { get { return _connection; } }

        public IDbDataAdapter Adapter
        {
            get { return _provider.CreateDataAdapter(); }
        }

        public ProjectContext() {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public ProjectContext(string connectionName)
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

        private void Close()
        {

            if (_connection != null)
            {

                if (_connection != null && _connection.State == ConnectionState.Open)
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