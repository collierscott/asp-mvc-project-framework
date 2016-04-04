using Infrastructure.Data.Abstract;
using Infrastructure.Data.Notify;

namespace Project.Application.Repositories.Abstract
{

    public interface IMenuRepository
    {
        Notifications Messages { get; set; }
    }

}
