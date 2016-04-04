using Project.Application.Repositories.Abstract;
using Infrastructure.Data;
using log4net;

namespace Project.Application.Repositories
{

    public class ProjectRepository : GenericRepository, IProjectRepository
    {

        // ReSharper disable once UnusedMember.Local
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private IDatabaseContext _context;
        //private DbConnection _connection;
        //private bool _disposed = false;

        public ProjectRepository(string context)
            : base(context)
        { }

    }

}
