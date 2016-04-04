using System.Collections.Generic;

using Project.Application.Models.Menus;

namespace Project.Application.Services.Abstract
{

    public interface IMenuService
    {
        IEnumerable<MenuItem> GetTopMenuItems(object parameters);
    }

}
