using System;
using Project.Application.Contexts;
using Project.Application.Repositories.Abstract;
using Infrastructure.Data.Notify;
using log4net;

namespace Project.Application.Repositories
{

    public class MenuRepository : IMenuRepository
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MenuContext _context;
        //private IDbConnection _connection;
        private bool _disposed;

        public Notifications Messages { get; set; }

        public MenuRepository(string context)
        {
            if (!string.IsNullOrWhiteSpace(context))
            {

                _context = new MenuContext(context);

                if (_context.Messages != null)
                {
                    Messages = _context.Messages;
                }

                //_connection = _context.Initialize();

            } else
            {
                _context = new MenuContext();
            }

        }
        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        #endregion
    }
}
